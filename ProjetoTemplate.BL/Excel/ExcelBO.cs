using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ProjetoTemplate.BL.Excel
{
    public class ExcelBO : IExcelBO
    {
        public async Task<FileDownloadDTO> ExportExcel<T>(List<T> result, string fileName)
        {
            FileDownloadDTO file = await CreateExcel(result, fileName);

            return file;
        }

        public async Task<FileDownloadDTO> CreateExcel<T>(List<T> result, string fileName)
        {
            var file = new FileDownloadDTO
            {
                FileName = fileName
            };

            var excelAux = new ExcelAux();
            var pathString = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Storage");

            if (!Directory.Exists(pathString))
                Directory.CreateDirectory(pathString);

            var doc = Path.Combine(pathString, file.FileName);
            file.Memory = new MemoryStream();

            excelAux.CreateExcel(doc, result, new StyleSheetSettings());

            using (var stream = System.IO.File.OpenRead(doc))
            {
                await stream.CopyToAsync(file.Memory);
            }

            file.Memory.Position = 0;
            file.FileExtension = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            System.IO.File.Delete(doc);
            return file;
        }
    }
}
