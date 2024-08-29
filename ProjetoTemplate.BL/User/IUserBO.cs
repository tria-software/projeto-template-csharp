using ProjetoTemplate.BL.Excel;
using ProjetoTemplate.Domain.DTO;
using ProjetoTemplate.Domain.DTO.User;
using ProjetoTemplate.Domain.Helpers;
using System.Threading.Tasks;

namespace ProjetoTemplate.BL
{
    public interface IUserBO
    {
        Task<GridViewData<UserListDTO>> GetAll(UserFilterDTO filter);
        Task<bool> SaveUpdate(UserDTO user);
        Task<bool> Delete(long userID);
        Task<bool> ActivateDisable(ActivateDisableDeleteDTO model);
        Task<UserDTO> GetById(long id);
        Task<bool> VerifyExistsEmail(string email, long? id);
        Task<bool> UpdatePassword(long userID, string password, string oldpassword, bool isFirstAccess);
        Task<FileDownloadDTO> Export2Excel(UserFilterDTO filter);
        Task<bool> SendEmail(string email);
    }
}
