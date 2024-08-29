namespace ProjetoTemplate.Domain.DTO.User
{
    public class UserFilterDTO : PageDTO
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public bool? Status { get; set; }
    }
}
