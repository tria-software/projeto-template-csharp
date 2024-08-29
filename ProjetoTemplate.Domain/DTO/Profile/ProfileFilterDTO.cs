namespace ProjetoTemplate.Domain.DTO.Profile
{
    public class ProfileFilterDTO
    {
        public string Description { get; set; }
        public bool? Status { get; set; }
        public int PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string Search { get; set; }
    }
}
