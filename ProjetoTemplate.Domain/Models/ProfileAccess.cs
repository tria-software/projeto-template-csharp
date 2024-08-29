namespace ProjetoTemplate.Domain.Models
{
    public class ProfileAccess
    {
        public long Id { get; set; }
        public string Module { get; set; }
        public long ProfileId { get; set; }
        public Profile Profile { get; set; }
    }
}
