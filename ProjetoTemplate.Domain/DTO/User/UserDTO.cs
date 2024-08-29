namespace ProjetoTemplate.Domain.DTO.User
{
    public class UserDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public long ProfileId { get; set; }
        public bool AccessAllMultipliers { get; set; }
        public bool FirstAccess { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
