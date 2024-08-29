using System;
using System.ComponentModel;

namespace ProjetoTemplate.Domain.DTO.Profile
{
    public class ProfileListDTO
    {
        public long Id { get; set; }

        [DisplayName("Descrição")]
        public string Description { get; set; }

        [DisplayName("Status")]
        public bool Status { get; set; }

        [DisplayName("Acesso a todas as telas")]
        public bool AccessAllModules { get; set; }

        [DisplayName("Perfil Admin")]
        public bool IsProfileAdmin { get; set; }

        [DisplayName("Data de Criação")]
        public DateTime? CreateDate { get; set; }
    }
}
