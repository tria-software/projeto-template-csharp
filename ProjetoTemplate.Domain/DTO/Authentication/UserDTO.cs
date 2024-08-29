namespace ProjetoTemplate.Domain.DTO.Authentication
{
    public class UserDTO
    {
        public long Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool AccessAllModules { get; set; }
        public string Profile { get; set; }
        public bool IsProfileAdmin { get; set; }
        public bool IsFirstAccess { get; set; }
        public List<string> ModuleAccess { get; set; }

    }
}
