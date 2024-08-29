using System.Text.Json.Serialization;

namespace ProjetoTemplate.Domain.DTO.LayoutLot
{
    public class CreateLayoutDTO
    {
        public long Id { get; set; }
        public string Table { get; set; }
        public string Name { get; set; }
        public List<string> Columns { get; set; }
        [JsonIgnore]
        public long UserId { get; set; }
    }
}
