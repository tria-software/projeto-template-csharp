namespace ProjetoTemplate.Domain.DTO
{
    public class ImportReponseDTO
    {
        public int TotalLines { get; set; } = 0;
        public int TotalLinesAdded { get; set; } = 0;
        public int TotalLinesUpdated { get; set; } = 0;
        public int TotalLinesFailure { get; set; } = 0;
        public string Message { get; set; }
    }
}
