using ProjetoTemplate.Domain.DTO.LayoutLot;
using ProjetoTemplate.Domain.Helpers;

namespace ProjetoTemplate.BL.LayoutLot
{
    public interface ILayoutColumnsBO
    {
        Task<GridViewData<ListLayoutColumnsDTO>> GetAll(long userId, string table);
        Task<long> SaveUpdate(CreateLayoutDTO dto);
        Task<bool> DeleteLayout(long id);
    }
}
