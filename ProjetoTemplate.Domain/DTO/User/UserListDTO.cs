using System.ComponentModel;

namespace ProjetoTemplate.Domain.DTO.User
{
    public class UserListDTO
    {
        public long Id { get; set; }

        [DisplayName("Nome")]
        public string Name { get; set; }

        [DisplayName("Sobrenome")]
        public string LastName { get; set; }

        [DisplayName("Login")]
        public string Login { get; set; }

        [DisplayName("E-mail")]
        public string Email { get; set; }

        [DisplayName("Perfil")]
        public string Profile { get; set; }

        [DisplayName("Accesso a todos os Multiplicadores")]
        public bool AccessAllMultipliers { get; set; }

        [DisplayName("Status")]
        public bool Status { get; set; }
    }
}
