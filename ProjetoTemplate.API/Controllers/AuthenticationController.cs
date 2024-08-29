using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoTemplate.BL.Authentication;
using ProjetoTemplate.Domain.DTO.Authentication;

namespace ProjetoTemplate.API.Controllers
{
    [Route("[controller]")]
    [AllowAnonymous]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationBO authenticationBO;

        public AuthenticationController(IAuthenticationBO _authenticationBO)
        {
            authenticationBO = _authenticationBO;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] AuthenticationDTO loginDTO)
        {
            try
            {
                var result = await authenticationBO.Login(loginDTO);
                if (result != null)
                    return Ok(new
                    {
                        success = result.Success,
                        message = result.Message,
                        token = result.Token
                    });
                else
                    return Ok(null);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(string email)
        {
            try
            {
                var result = await authenticationBO.ChangePassword(email);
                if (result != null)
                    return Ok(new
                    {
                        success = result.Success,
                        message = result.Message,
                        token = result.Token
                    });
                else
                    return Ok(null);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("VerifyLinkChangePassword")]
        public async Task<IActionResult> VerifyLinkChangePassword(string hash)
        {
            try
            {
                var result = await authenticationBO.VerifyLinkChangePassword(hash);
                if (result != null)
                    return Ok(new
                    {
                        success = result.Success,
                        message = result.Message,
                        token = result.Token
                    });
                else
                    return Ok(null);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] ChangePasswordDTO change)
        {
            try
            {
                var result = await authenticationBO.UpdatePassword(change.Hash, change.Password);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
