namespace ProjetoTemplate.Domain.Models
{
    public class ChangePassword
    {
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public long UserId { get; set; }
        public bool Used { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Hash { get; set; }
    }
}
