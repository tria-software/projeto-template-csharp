using DocumentFormat.OpenXml.Spreadsheet;

namespace ProjetoTemplate.BL.Excel
{
    public class StyleSheetSettings
    {
        public string NameTab { get; set; } = "Planilha1";

        public int FontSizeHeader { get; set; } = 10;

        public int FontSizeBody { get; set; } = 10;

        public string ColorFontHeader { get; set; } = "FFFFFF";

        public string ColorFontBody { get; set; } = "000000";

        public string ColorBackgroundHeader { get; set; } = "7D7D7D";

        public string ColorBackgroundBody { get; set; } = "FFFFFF";

        public BorderStyleValues LeftBorderHeader { get; set; } = BorderStyleValues.Thin;

        public BorderStyleValues RightBorderHeader { get; set; } = BorderStyleValues.Thin;

        public BorderStyleValues TopBorderHeader { get; set; } = BorderStyleValues.Thin;

        public BorderStyleValues BottomBorderHeader { get; set; } = BorderStyleValues.Thin;

        public BorderStyleValues LeftBorderBody { get; set; } = BorderStyleValues.Thin;

        public BorderStyleValues RightBorderBody { get; set; } = BorderStyleValues.Thin;

        public BorderStyleValues TopBorderBody { get; set; } = BorderStyleValues.Thin;

        public BorderStyleValues BottomBorderBody { get; set; } = BorderStyleValues.Thin;

        public HorizontalAlignmentValues AligHeaderHorizontal { get; set; } = HorizontalAlignmentValues.Center;

        public HorizontalAlignmentValues AligBodyHorizontal { get; set; } = HorizontalAlignmentValues.Center;

        public VerticalAlignmentValues AligHeaderVertical { get; set; } = VerticalAlignmentValues.Center;

        public VerticalAlignmentValues AligBodyVertical { get; set; } = VerticalAlignmentValues.Center;
    }
}
