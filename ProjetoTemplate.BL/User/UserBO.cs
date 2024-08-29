using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjetoTemplate.BL;
using ProjetoTemplate.BL.Excel;
using ProjetoTemplate.BL.Security;
using ProjetoTemplate.BL.SendEmail;
using ProjetoTemplate.Domain.DTO;
using ProjetoTemplate.Domain.DTO.User;
using ProjetoTemplate.Domain.Helpers;
using ProjetoTemplate.Domain.Models;
using ProjetoTemplate.Repository;

namespace ProjetoTemplate.Data.Repository
{
    public class UserBO : IUserBO
    {
        private readonly ProjetoTemplateDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISendEmailBO _sendEmail;
        private readonly ISecurityBO _security;
        private readonly string _url;

        public UserBO(
            ProjetoTemplateDbContext context,
            IMapper mapper,
            ISendEmailBO sendEmail,
            ISecurityBO security,
            AppSettingsConfig appSettingsConfig)
        {
            _context = context;
            _mapper = mapper;
            _sendEmail = sendEmail;
            _security = security;
            _url = appSettingsConfig.LinkApplication;
        }

        public async Task<GridViewData<UserListDTO>> GetAll(UserFilterDTO filter)
        {
            try
            {
                var query = _context.Users
                    .Include(x => x.Profile)
                    .Where(x =>
                    (string.IsNullOrEmpty(filter.Name) || x.Name.Contains(filter.Name)) &&
                    (string.IsNullOrEmpty(filter.LastName) || x.LastName.Contains(filter.LastName)) &&
                    (string.IsNullOrEmpty(filter.Email) || x.Email.Contains(filter.Email)) &&
                    (string.IsNullOrEmpty(filter.Login) || x.Login.Contains(filter.Login)) &&
                    (filter.Status == null || x.Status == filter.Status)).AsQueryable();

                if (!string.IsNullOrEmpty(filter.Search))
                {
                    query = query.Where(x =>
                                       x.Name.ToLower().Contains(filter.Search.ToLower()) ||
                                       x.LastName.ToLower().Contains(filter.Search.ToLower()) ||
                                       x.Email.ToLower().Contains(filter.Search.ToLower()) ||
                                       x.Login.ToLower().Contains(filter.Search.ToLower())).AsQueryable();
                }

                var queryResult = from q in query.OrderBy(o => o.CreateDate)
                                  select new UserListDTO
                                  {
                                      Id = q.Id,
                                      Name = q.Name,
                                      LastName = q.LastName,
                                      Email = q.Email,
                                      Login = q.Login,
                                      Status = q.Status,
                                      Profile = q.Profile.Description
                                  };

                queryResult = queryResult.AsQueryable().RWSPaginate(filter.PageIndex, filter.PageSize ?? 10);

                var gvResult = new GridViewData<UserListDTO>()
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

        public async Task<bool> SaveUpdate(UserDTO userDto)
        {
            try
            {
                var sendEmail = true;
                var user = _mapper.Map<User>(userDto);
                if (user != null)
                {
                    if (user.Id != 0)
                    {
                        sendEmail = false;
                        _context.Update(user);
                    }
                    else
                    {
                        user.FirstAccess = true;
                        user.Password = _security.Encrypt(user.Password);
                        _context.Add(user);
                    }

                    await _context.SaveChangesAsync();

                    if (sendEmail && !string.IsNullOrEmpty(user.Email))
                    {
                        var html = HtmlToEmail(user.Name, user.Login, user.Email, userDto.Password);
                        await _sendEmail.SendEmail(user.Email, "ProjetoTemplate - Acesso ao APP", html);
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

        public async Task<bool> Delete(long userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);

                if (user != null)
                {
                    var changePassword = _context.ChangePassword.Where(x => x.UserId == userId);
                    if (changePassword.Any())
                        _context.RemoveRange(changePassword);

                    _context.Remove(user);
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

        public async Task<UserDTO> GetById(long userId)
        {
            try
            {
                var user = await _context.Users.FirstAsync(x => x.Id == userId);

                if (user != null)
                    return _mapper.Map<UserDTO>(user);

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> VerifyExistsEmail(string email, long? id)
        {
            try
            {
                if (id.HasValue)
                    return await _context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower() && x.Id != id.Value);
                else
                    return await _context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string HtmlToEmail(string name, string login, string email, string password)
        {
            var html = string.Empty;

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TemplateEmail", "acesso_sistema.html");

            if (System.IO.File.Exists(filePath))
            {
                html = System.IO.File.ReadAllText(filePath);
                html = html.Replace("%NAME%", name);
                html = html.Replace("%LOGIN%", login);
                html = html.Replace("%EMAIL%", email);
                html = html.Replace("%PASSWORD%", password);
                html = html.Replace("%LINK%", _url);
            }

            return html;
        }

        public async Task<bool> UpdatePassword(long userId, string password, string oldpassword, bool isFirstAccess)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);

                if (user != null)
                {
                    if (!isFirstAccess)
                    {
                        if (_security.Encrypt(oldpassword) != user.Password)
                            return false;
                    }

                    user.Password = _security.Encrypt(password);
                    user.LastUpdateDate = DateTimeBrazil.Now();
                    user.FirstAccess = false;

                    _context.Update(user);

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

        public async Task<FileDownloadDTO> Export2Excel(UserFilterDTO filter)
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
                var model = await _context.Users.FindAsync(dto.Id);

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

        public async Task<bool> SendEmail(string email)
        {
            var html = HtmlToEmail("TESTE", "lucasmachadopaulista@hotmail.com", "lucasmachadopaulista@hotmail.com", "123");
            await _sendEmail.SendEmail(email, "ProjetoTemplate - Acesso ao APP", html);

            return true;
        }
    }
}
