using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace ProjetoTemplate.BL.Excel
{
    public class ExcelAux
    {
        public void CreateExcel<T>(string doc, List<T> lstBody, StyleSheetSettings style)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(doc, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                WorkbookStylesPart stylePart = workbookPart.AddNewPart<WorkbookStylesPart>();
                stylePart.Stylesheet = GenerateStylesheet(style);
                stylePart.Stylesheet.Save();

                Type type = typeof(T);
                var propriets = type.GetProperties();

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = style.NameTab };
                sheets.Append(sheet);

                workbookPart.Workbook.Save();

                SheetData sheetData = new SheetData();

                /*HEADER*/
                Row row = new Row();
                var lstCell = new List<Cell>();
                var getPropertiesForExcel = type.GetProperties().Where(x => x.CustomAttributes.Count() > 0);
                var countPropertiesForExcel = 0;
                var itensRemoved = new List<int>();

                foreach (var item in getPropertiesForExcel)
                {
                    if (item.CustomAttributes.Count() > 0)
                    {
                        if (item.GetCustomAttributes(typeof(DisplayNameAttribute), true).Count() > 0)
                        {
                            var attribute = item.GetCustomAttributes(typeof(DisplayNameAttribute), true)
                                    .Cast<DisplayNameAttribute>().Single();

                            string displayName = attribute.DisplayName;
                            lstCell.Add(ConstructCell(displayName, CellValues.String, 2));

                            countPropertiesForExcel++;
                        }
                    }
                }

                row.Append(lstCell);
                sheetData.AppendChild(row);

                /*BODY*/
                var result = lstBody;

                var dtAux = new DataTableAux();
                var dtResult = dtAux.ListToDataTable(result);

                foreach (DataRow dr in dtResult.Rows)
                {
                    lstCell = new List<Cell>();
                    row = new Row();

                    for (int i = 0; i < countPropertiesForExcel; i++)
                    {
                        if (itensRemoved.Any(p => p == i))
                            continue;

                        var valueType = dr[i].GetType();
                        var value = dr[i].ToString();
                        if (valueType.Name == "Decimal")
                            value = value.Replace(".", ",");

                        if (!decimal.TryParse(dr[i].ToString(), out _) && DateTime.TryParse(dr[i].ToString(), out _) && value.Length > 7 && !TimeSpan.TryParse(dr[i].ToString(), out _))
                        {
                            var date = DateTime.Parse(dr[i].ToString());
                            value = $"{date.Day:D2}/{date.Month:D2}/{date.Year}";
                        }

                        lstCell.Add(ConstructCell(dr[i] == DBNull.Value ? string.Empty : value, CellValues.String, 1));
                    }

                    if (lstCell.Count > 0)
                    {
                        row.Append(lstCell);
                        sheetData.AppendChild(row);
                    }
                }

                Columns columns = AutoSize(sheetData);
                worksheetPart.Worksheet.Append(columns);
                worksheetPart.Worksheet.Append(sheetData);
                worksheetPart.Worksheet.Save();
            }
        }

        private Columns AutoSize(SheetData sheetData)
        {
            var maxColWidth = GetMaxCharacterWidth(sheetData);

            Columns columns = new Columns();

            double maxWidth = 10;

            foreach (var item in maxColWidth)
            {
                double width = Math.Truncate((item.Value * maxWidth + 5) / maxWidth * 256) / 256;

                double pixels = Math.Truncate(((256 * width + Math.Truncate(128 / maxWidth)) / 256) * maxWidth);

                double charWidth = Math.Truncate((pixels - 5) / maxWidth * 100 + 0.5) / 100;

                Column col = new Column() { BestFit = true, Min = (UInt32)(item.Key + 1), Max = (UInt32)(item.Key + 1), CustomWidth = true, Width = (DoubleValue)width };
                columns.Append(col);
            }

            return columns;
        }

        private Dictionary<int, int> GetMaxCharacterWidth(SheetData sheetData)
        {
            Dictionary<int, int> maxColWidth = new Dictionary<int, int>();
            var rows = sheetData.Elements<Row>();
            UInt32[] numberStyles = new UInt32[] { 5, 6, 7, 8 };
            UInt32[] boldStyles = new UInt32[] { 1, 2, 3, 4, 6, 7, 8 };

            foreach (var r in rows)
            {
                var cells = r.Elements<Cell>().ToArray();

                for (int i = 0; i < cells.Length; i++)
                {
                    var cell = cells[i];
                    var cellValue = cell.CellValue == null ? string.Empty : cell.CellValue.InnerText;
                    var cellTextLength = cellValue.Length;

                    if (cell.StyleIndex != null && numberStyles.Contains(cell.StyleIndex))
                    {
                        int thousandCount = (int)Math.Truncate((double)cellTextLength / 4);

                        cellTextLength += (3 + thousandCount);
                    }

                    if (cell.StyleIndex != null && boldStyles.Contains(cell.StyleIndex))
                        cellTextLength += 1;

                    if (maxColWidth.ContainsKey(i))
                    {
                        var current = maxColWidth[i];
                        if (cellTextLength > current)
                        {
                            maxColWidth[i] = cellTextLength;
                        }
                    }
                    else
                        maxColWidth.Add(i, cellTextLength);
                }
            }

            return maxColWidth;
        }

        public Stylesheet GenerateStylesheet(StyleSheetSettings style)
        {
            Stylesheet styleSheet = null;

            Fonts fonts = new Fonts(
                /*HEADER*/
                new Font(
                    new Bold(),
                    new FontSize() { Val = style.FontSizeHeader },
                    new Color() { Rgb = style.ColorFontHeader.Replace("#", "") }
                ),
                /*BODY*/
                new Font(
                    new FontSize() { Val = style.FontSizeBody },
                    new Color() { Rgb = style.ColorFontBody.Replace("#", "") },
                    new BackgroundColor() { Rgb = style.ColorBackgroundBody.Replace("#", "") }
                ));

            Alignment aligHeader = new Alignment
            {
                WrapText = true,
                Horizontal = style.AligHeaderHorizontal,
                Vertical = style.AligHeaderVertical
            };

            Alignment aligBody = new Alignment
            {
                WrapText = false,
                Horizontal = style.AligBodyHorizontal,
                Vertical = style.AligBodyVertical
            };

            Fills fills = new Fills(
                    new Fill(new PatternFill() { PatternType = PatternValues.None }),
                    new Fill(new PatternFill() { PatternType = PatternValues.Gray125 }),
                    /*HEADER*/
                    new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = style.ColorBackgroundHeader.Replace("#", "") } })
                    { PatternType = PatternValues.Solid }),
                    /*BODY*/
                    new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = style.ColorBackgroundBody.Replace("#", "") } })
                    { PatternType = PatternValues.Solid })
                );

            Borders borders = new Borders(
                    /*HEADER*/
                    new Border(),
                    new Border(
                        new LeftBorder(new Color() { Auto = true }) { Style = style.LeftBorderHeader },
                        new RightBorder(new Color() { Auto = true }) { Style = style.RightBorderHeader },
                        new TopBorder(new Color() { Auto = true }) { Style = style.TopBorderHeader },
                        new BottomBorder(new Color() { Auto = true }) { Style = style.BottomBorderHeader },
                        new DiagonalBorder()),
                    /*BODY*/
                    new Border(
                        new LeftBorder(new Color() { Auto = true }) { Style = style.LeftBorderBody },
                        new RightBorder(new Color() { Auto = true }) { Style = style.RightBorderBody },
                        new TopBorder(new Color() { Auto = true }) { Style = style.TopBorderBody },
                        new BottomBorder(new Color() { Auto = true }) { Style = style.BottomBorderBody },
                        new DiagonalBorder())
                );

            CellFormats cellFormats = new CellFormats(
                    new CellFormat(),
                    new CellFormat { FontId = 1, FillId = 3, BorderId = 1, ApplyBorder = true, Alignment = aligBody },
                    new CellFormat { FontId = 0, FillId = 2, BorderId = 2, ApplyFill = true, ApplyBorder = true, Alignment = aligHeader }
                );

            styleSheet = new Stylesheet(fonts, fills, borders, cellFormats);

            return styleSheet;
        }

        public Cell ConstructCell(string value, CellValues dataType, uint styleIndex = 0)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType),
                StyleIndex = styleIndex
            };
        }
    }
}
