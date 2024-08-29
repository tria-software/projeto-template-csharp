using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjetoTemplate.BL.Excel;
using ProjetoTemplate.Domain.DTO;
using ProjetoTemplate.Domain.DTO.Profile;
using ProjetoTemplate.Domain.Helpers;
using ProjetoTemplate.Domain.Models;
using ProjetoTemplate.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoTemplate.BL.Profile
{
    public class ProfileBO : IProfileBO
    {
        private readonly ProjetoTemplateDbContext _context;
        private readonly IMapper _mapper;

        public ProfileBO(
            ProjetoTemplateDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GridViewData<ProfileListDTO>> GetAll(ProfileFilterDTO filter)
        {
            try
            {
                var query = _context.Profile.Where(x =>
                    (string.IsNullOrEmpty(filter.Description) || x.Description.Contains(filter.Description)) &&
                    (filter.Status == null || x.Status == filter.Status)).AsQueryable();

                if (!string.IsNullOrEmpty(filter.Search))
                {
                    query = query.Where(x =>
                                       x.Description.ToLower().Contains(filter.Search.ToLower()));
                }

                var queryResult = from q in query.OrderBy(o => o.CreateDate)
                                  select new ProfileListDTO
                                  {
                                      Id = q.Id,
                                      Description = q.Description,
                                      Status = q.Status,
                                      AccessAllModules = q.AccessAllModules,
                                      CreateDate = q.CreateDate,
                                      IsProfileAdmin = q.IsAdmin
                                  };

                queryResult = filter.PageSize.HasValue ? queryResult.AsQueryable().RWSPaginate(filter.PageIndex, filter.PageSize ?? 10) : queryResult;

                var gvResult = new GridViewData<ProfileListDTO>()
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

        public async Task<bool> SaveUpdate(ProfileDTO profileDto)
        {
            try
            {
                var profile = _mapper.Map<Domain.Models.Profile>(profileDto);
                if (profile != null)
                {
                    if (profile.Id != 0)
                        _context.Update(profile);
                    else
                        _context.Add(profile);

                    await _context.SaveChangesAsync();


                    var modulesRemove = _context.ProfileAccess.Where(x => x.ProfileId == profile.Id);
                    if (modulesRemove.Any())
                    {
                        _context.RemoveRange(modulesRemove);
                        await _context.SaveChangesAsync();
                    }

                    if (!profile.AccessAllModules)
                    {
                        var modulesAccess = new List<ProfileAccess>();

                        foreach (var module in profileDto.Modules)
                            modulesAccess.Add(new ProfileAccess { ProfileId = profile.Id, Module = module });

                        if (modulesAccess.Any())
                        {
                            _context.AddRange(modulesAccess);
                            await _context.SaveChangesAsync();
                        }
                    }

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BaseResponseDTO> DeleteProfile(long profileId)
        {
            try
            {
                var profile = await _context.Profile.FindAsync(profileId);

                if (profile != null)
                {
                    var userProfile = _context.Users.Any(x => x.ProfileId == profileId);
                    if (userProfile)
                        return new BaseResponseDTO { Success = false, Message = "Existem usuários com vinculo a esse Perfil, não é possível deletar." };

                    _context.Remove(profile);
                    await _context.SaveChangesAsync();

                    return new BaseResponseDTO { Success = true };
                }

                return new BaseResponseDTO { Success = false, Message = "Perfil não encontrado" };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BaseResponseDTO> ActivateDisable(ActivateDisableDeleteDTO dto)
        {
            try
            {
                var model = await _context.Profile.FindAsync(dto.Id);

                if (model != null)
                {
                    var userProfile = _context.Users.Any(x => x.ProfileId == dto.Id);
                    if (userProfile && model.Status)
                        return new BaseResponseDTO { Success = false, Message = "Existem usuários com vinculo a esse Perfil, não é possível inativar." };

                    model.DeleteDate = model.Status ? DateTimeBrazil.Now() : null;
                    model.LastUpdateDate = DateTimeBrazil.Now();
                    model.Status = !model.Status;

                    _context.Update(model);
                    await _context.SaveChangesAsync();

                    return new BaseResponseDTO { Success = true };
                }

                return new BaseResponseDTO { Success = false, Message = "Perfil não encontrado" };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ProfileDTO> GetById(long profileId)
        {
            try
            {
                var profile = await _context.Profile.Include(x => x.ProfileAccess).FirstAsync(x => x.Id == profileId);

                if (profile != null)
                    return _mapper.Map<ProfileDTO>(profile);

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<FileDownloadDTO> Export2Excel(ProfileFilterDTO filter)
        {
            try
            {
                filter.PageIndex = 0;
                filter.PageSize = int.MaxValue;

                var listResult = await GetAll(filter);
                var excel = new ExcelBO();
                return await excel.ExportExcel(listResult.Data, "Cultivar");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
