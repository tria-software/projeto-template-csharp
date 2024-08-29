using Microsoft.EntityFrameworkCore;
using ProjetoTemplate.BL.Jwt;
using ProjetoTemplate.BL.Security;
using ProjetoTemplate.BL.SendEmail;
using ProjetoTemplate.Domain.DTO.Authentication;
using ProjetoTemplate.Domain.Helpers;
using ProjetoTemplate.Domain.Models;
using ProjetoTemplate.Repository;

namespace ProjetoTemplate.BL.Authentication
{
    public class AuthenticationBO : IAuthenticationBO
    {
        private readonly IJwtFactory _jwtFactory;
        private readonly ProjetoTemplateDbContext _context;
        private readonly string _url;
        private readonly ISendEmailBO _sendEmail;
        private readonly ISecurityBO _security;

        public AuthenticationBO(
            IJwtFactory jwtFactory,
            ProjetoTemplateDbContext context,
            AppSettingsConfig appSettingsConfig,
            ISendEmailBO sendEmail,
            ISecurityBO security)
        {
            _jwtFactory = jwtFactory;
            _context = context;
            _url = appSettingsConfig.LinkApplication;
            _sendEmail = sendEmail;
            _security = security;
        }

        public async Task<ResultLoginDTO> Login(AuthenticationDTO model)
        {
            try
            {
                var result = new ResultLoginDTO();

                var userClaims = new UserDTO();
                var user = await _context.Users
                    .Include(x => x.Profile)
                        .ThenInclude(x => x.ProfileAccess)
                    .Where(x => x.Email == model.Email).FirstOrDefaultAsync();

                if (user != null)
                {
                    var cripto = _security.Encrypt(model.Password);
                    if (cripto != user.Password)
                    {
                        result.Message = "Senha inválida";
                        result.Success = false;
                        return result;
                    }

                    if (!user.Status)
                    {
                        result.Message = "Seu cadastro está inativo no sistema";
                        result.Success = false;
                        return result;
                    }

                    userClaims.Id = user.Id;
                    userClaims.Name = $"{user.Name} {user.LastName}";
                    userClaims.Login = user.Login;
                    userClaims.Email = user.Email;
                    userClaims.Profile = user.Profile.Description;
                    userClaims.IsProfileAdmin = user.Profile.IsAdmin;
                    userClaims.AccessAllModules = user.Profile.AccessAllModules;
                    userClaims.ModuleAccess = user.Profile.ProfileAccess.Select(x => x.Module).ToList();

                    var token = _jwtFactory.GenerateEncodedToken(userClaims);
                    result.Token = token;
                    result.Success = true;
                    return result;
                }
                else
                {
                    result.Message = "E-mail não encontrado no sistema";
                    result.Success = false;
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResultLoginDTO> ChangePassword(string email)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

                if (user != null)
                {
                    var change = new ChangePassword
                    {
                        CreateDate = DateTimeBrazil.Now(),
                        ExpirationDate = DateTimeBrazil.Now().AddDays(1),
                        Hash = Guid.NewGuid().ToString(),
                        Used = false,
                        UserId = user.Id
                    };

                    _context.Add(change);
                    await _context.SaveChangesAsync();

                    var html = HtmlToEmail(user.Name, user.Email, change.Hash);
                    await _sendEmail.SendEmail(user.Email, "ProjetoTemplate - Alteração de Senha", html);

                    return new ResultLoginDTO
                    {
                        Success = true
                    };
                }
                else
                    return new ResultLoginDTO
                    {
                        Success = false,
                        Message = "Usuário não encontardo com esse e-mail"
                    };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string HtmlToEmail(string name, string email, string hash)
        {
            var html = string.Empty;

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TemplateEmail", "recuperacao_senha.html");

            if (System.IO.File.Exists(filePath))
            {
                html = System.IO.File.ReadAllText(filePath);
                html = html.Replace("%NAME%", name);
                html = html.Replace("%EMAIL%", email);
                html = html.Replace("%LINK%", $"{_url}/#/forgot-password/{hash}");
            }

            return html;
        }

        public async Task<ResultLoginDTO> VerifyLinkChangePassword(string hash)
        {
            try
            {
                var link = await _context.ChangePassword.FirstOrDefaultAsync(x => x.Hash == hash);

                if (link != null)
                {
                    if (link.Used || DateTimeBrazil.Now() > link.ExpirationDate)
                        return new ResultLoginDTO
                        {
                            Success = false,
                            Message = "A solicitação não é mais válida!"
                        };
                    else
                        return new ResultLoginDTO
                        {
                            Success = true
                        };
                }
                else
                    return new ResultLoginDTO
                    {
                        Success = false,
                        Message = "A solicitação não é mais válida!"
                    };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdatePassword(string hash, string password)
        {
            try
            {
                var change = await _context.ChangePassword.FirstOrDefaultAsync(x => x.Hash == hash && !x.Used);

                if (change != null)
                {
                    var user = await _context.Users.FindAsync(change.UserId);

                    user.Password = _security.Encrypt(password);
                    user.LastUpdateDate = DateTimeBrazil.Now();

                    change.Used = true;

                    _context.Update(user);
                    _context.Update(change);

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
