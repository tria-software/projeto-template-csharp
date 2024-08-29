using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetoTemplate.BL.SendEmail
{
    public interface ISendEmailBO
    {
        Task<bool> SendEmail(string email, string subject, string message, List<string> listCCEmail = null);
    }
}
