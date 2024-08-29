using ProjetoTemplate.BL.Excel;
using ProjetoTemplate.Domain.DTO;
using ProjetoTemplate.Domain.DTO.Profile;
using ProjetoTemplate.Domain.Helpers;
using System.Threading.Tasks;

namespace ProjetoTemplate.BL.Profile
{
    public interface IProfileBO
    {
        Task<GridViewData<ProfileListDTO>> GetAll(ProfileFilterDTO filter);
        Task<bool> SaveUpdate(ProfileDTO model);
        Task<BaseResponseDTO> DeleteProfile(long id);
        Task<BaseResponseDTO> ActivateDisable(ActivateDisableDeleteDTO model);
        Task<ProfileDTO> GetById(long id);
        Task<FileDownloadDTO> Export2Excel(ProfileFilterDTO filter);
    }
}
