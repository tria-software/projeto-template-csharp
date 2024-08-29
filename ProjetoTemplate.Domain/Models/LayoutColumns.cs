namespace ProjetoTemplate.Domain.Models
{
    public class LayoutColumns : BaseModel
    {
        public string Name { get; set; }
        public string Table { get; set; }
        public string Columns { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
    }
}
