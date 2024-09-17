using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjetoTemplate.BL;
using ProjetoTemplate.BL.Excel;
using ProjetoTemplate.Domain.DTO;
using ProjetoTemplate.Domain.DTO.EntityName;
using ProjetoTemplate.Domain.Helpers;
using ProjetoTemplate.Domain.Models;
using ProjetoTemplate.Repository;

namespace ProjetoTemplate.Data.Repository
{
    public class EntityNameBO : IEntityNameBO
    {
        private readonly ProjetoTemplateDbContext _context;
        private readonly IMapper _mapper;

        public EntityNameBO(
            ProjetoTemplateDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GridViewData<EntityNameListDTO>> GetAll(EntityNameFilterDTO filter)
        {
            try
            {
                var query = _context.EntityName.Where(x => x.Id > 1).AsQueryable();

                if (!string.IsNullOrEmpty(filter.Search))
                {
                    query = query.Where(x => x.Id.ToString().Contains(filter.Search.ToLower()));
                }

                var queryResult = from q in query.OrderBy(o => o.CreateDate)
                                  select new EntityNameListDTO
                                  {
                                      Id = q.Id,
                                      CreateDate = q.CreateDate,
                                  };

                queryResult = queryResult.AsQueryable().RWSPaginate(filter.PageIndex, filter.PageSize ?? 10);

                var gvResult = new GridViewData<EntityNameListDTO>()
                {
                    Count = await query.CountAsync(),
                    Data = queryResult.ToList()
                };

                return gvResult;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SaveUpdate(EntityNameDTO entityNameDto)
        {
            try
            {
                var entityName = _mapper.Map<EntityName>(entityNameDto);
                if (entityName != null)
                {
                    if (entityName.Id != 0)
                        _context.Update(entityName);                    
                    else
                        _context.Add(entityName);
                    
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

        public async Task<bool> Delete(long entityNameId)
        {
            try
            {
                var entityName = await _context.EntityName.FindAsync(entityNameId);

                if (entityName != null)
                {
                    _context.Remove(entityName);
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

        public async Task<EntityNameDTO> GetById(long entityNameId)
        {
            try
            {
                var entityName = await _context.EntityName.FirstAsync(x => x.Id == entityNameId);

                if (entityName != null)
                    return _mapper.Map<EntityNameDTO>(entityName);

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<FileDownloadDTO> Export2Excel(EntityNameFilterDTO filter)
        {
            try
            {
                filter.PageIndex = 0;
                filter.PageSize = int.MaxValue;

                var listResult = await GetAll(filter);
                var excel = new ExcelBO();
                return await excel.ExportExcel(listResult.Data, "Usuários");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ActivateDisable(ActivateDisableDeleteDTO dto)
        {
            try
            {
                var model = await _context.EntityName.FindAsync(dto.Id);

                if (model != null)
                {
                    model.DeleteDate = model.Status ? DateTimeBrazil.Now() : null;
                    model.LastUpdateDate = DateTimeBrazil.Now();
                    model.Status = !model.Status;

                    _context.Update(model);
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
