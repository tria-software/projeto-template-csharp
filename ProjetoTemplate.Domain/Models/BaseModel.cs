using ProjetoTemplate.Domain.Helpers;

namespace ProjetoTemplate.Domain.Models
{
    public class BaseModel
    {
        public long Id { get; set; }
        public DateTime CreateDate { get; set; } = DateTimeBrazil.Now();
        public DateTime? LastUpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string? CreatedBy { get; set; } = "System";
        public string? LastUpdateBy { get; set; }
    }
}
