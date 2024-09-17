using System.ComponentModel;

namespace ProjetoTemplate.Domain.DTO.EntityName
{
    public class EntityNameListDTO
    {
        public long Id { get; set; }

        [DisplayName("Status")]
        public bool Status { get; set; }

        [DisplayName("Data Criação")]
        public DateTime? CreateDate { get; set; }
    }
}
