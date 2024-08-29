using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjetoTemplate.Domain.DTO.LayoutLot;
using ProjetoTemplate.Domain.Helpers;
using ProjetoTemplate.Repository;
using System.Text.Json;

namespace ProjetoTemplate.BL.LayoutLot
{
    public class LayoutColumnsBO : ILayoutColumnsBO
    {
        private readonly ProjetoTemplateDbContext _context;
        private readonly IMapper _mapper;

        public LayoutColumnsBO(
            ProjetoTemplateDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GridViewData<ListLayoutColumnsDTO>> GetAll(long userId, string table)
        {
            try
            {
                var query = _context.LayoutColumns
                    .Include(p => p.User)
                    .Where(p => p.User.Id == userId && p.Table == table)
                    .AsQueryable();

                var queryResult = from q in query.OrderBy(o => o.CreateDate)
                                  select new LayoutColumnsDTO
                                  {
                                      Id = q.Id,
                                      Columns = q.Columns,
                                      Name = q.Name,
                                  };

                var result = new List<ListLayoutColumnsDTO>();

                foreach (var item in queryResult)
                {
                    result.Add(new ListLayoutColumnsDTO
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Columns = JsonSerializer.Deserialize<List<string>>(item.Columns)
                    });
                }

                var gvResult = new GridViewData<ListLayoutColumnsDTO>()
                {
                    Count = await query.CountAsync(),
                    Data = result
                };

                return gvResult;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<long> SaveUpdate(CreateLayoutDTO dto)
        {
            try
            {
                var layoutLot = new Domain.Models.LayoutColumns()
                {
                    Id = dto.Id,
                    Columns = JsonSerializer.Serialize(dto.Columns),
                    Name = dto.Name,
                    CreateDate = DateTimeBrazil.Now(),
                    LastUpdateDate = DateTimeBrazil.Now(),
                    UserId = dto.UserId,
                    Table = dto.Table,
                };

                if (layoutLot != null)
                {
                    if (layoutLot.Id != 0)
                        _context.Update(layoutLot);
                    else
                        _context.Add(layoutLot);

                    await _context.SaveChangesAsync();

                    return layoutLot.Id;
                }

                return layoutLot.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteLayout(long id)
        {
            try
            {
                var layout = _context.LayoutColumns.FirstOrDefault(p => p.Id == id);

                if (layout != null)
                {
                    _context.LayoutColumns.Remove(layout);
                    await _context.SaveChangesAsync();

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
