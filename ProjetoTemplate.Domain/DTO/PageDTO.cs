namespace ProjetoTemplate.Domain.DTO
{
    public class PageDTO
    {
        public int PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string Search { get; set; }
    }
}
