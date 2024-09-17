using ProjetoTemplate.BL.Excel;
using ProjetoTemplate.Domain.DTO;
using ProjetoTemplate.Domain.DTO.EntityName;
using ProjetoTemplate.Domain.Helpers;

namespace ProjetoTemplate.BL
{
    public interface IEntityNameBO
    {
        Task<GridViewData<EntityNameListDTO>> GetAll(EntityNameFilterDTO filter);
        Task<bool> SaveUpdate(EntityNameDTO entityName);
        Task<bool> Delete(long entityNameId);
        Task<bool> ActivateDisable(ActivateDisableDeleteDTO model);
        Task<EntityNameDTO> GetById(long id);
        Task<FileDownloadDTO> Export2Excel(EntityNameFilterDTO filter);
    }
}
