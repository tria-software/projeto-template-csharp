using System.IO;

namespace ProjetoTemplate.BL.Excel
{
    public class FileDownloadDTO
    {
        public MemoryStream Memory { get; set; }

        public string FileExtension { get; set; }

        public string FileName { get; set; }
    }
}
