namespace ProjetoTemplate.Domain.Models
{
    public class User : BaseModel
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool FirstAccess { get; set; } = true;
        public bool Status { get; set; }
        public long ProfileId { get; set; }
        public Profile Profile { get; set; }
    }
}
