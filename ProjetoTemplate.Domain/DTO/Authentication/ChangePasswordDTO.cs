namespace ProjetoTemplate.Domain.DTO.Authentication
{
    public class ChangePasswordDTO
    {
        public long UserId { get; set; }
        public string Hash { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
        public bool IsFirstAccess { get; set; }
    }
}
