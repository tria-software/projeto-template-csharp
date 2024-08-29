using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoTemplate.BL;
using ProjetoTemplate.Domain.DTO;
using ProjetoTemplate.Domain.DTO.Authentication;
using ProjetoTemplate.Domain.DTO.User;
using System;
using System.Threading.Tasks;

namespace ProjetoTemplate.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IUserBO _userBO;

        public UserController(IUserBO userBO)
        {
            _userBO = userBO;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll([FromBody] UserFilterDTO filter)
        {
            try
            {
                var result = await _userBO.GetAll(filter);
                return Ok(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha na busca dos Usuários.");
            }
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] Domain.DTO.User.UserDTO user)
        {
            try
            {
                var result = await _userBO.SaveUpdate(user);
                return Ok(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao salvar/atualizar Usuário.");
            }
        }

        [HttpPatch("Update")]
        public async Task<IActionResult> Update([FromBody] Domain.DTO.User.UserDTO user)
        {
            try
            {
                var result = await _userBO.SaveUpdate(user);
                return Ok(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao salvar/atualizar Usuário.");
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                var result = await _userBO.GetById(Id);
                return Ok(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha na busca do Usuário.");
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(long Id)
        {
            try
            {
                var result = await _userBO.Delete(Id);
                return Ok(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao deletar Usuário");
            }
        }

        [HttpPatch("ActivateDisable")]
        public async Task<IActionResult> ActivateDisable([FromBody] ActivateDisableDeleteDTO model)
        {
            try
            {
                var result = await _userBO.ActivateDisable(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao atualizar Usuário. Ex: {ex.Message}");
            }
        }

        [HttpGet("VerifyExistsEmail")]
        public async Task<IActionResult> VerifyExistsEmail(string email, long? id)
        {
            try
            {
                var result = await _userBO.VerifyExistsEmail(email, id);
                return Ok(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao verificar e-mail existente!");
            }
        }

        [HttpPost("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] ChangePasswordDTO change)
        {
            try
            {
                var result = await _userBO.UpdatePassword(change.UserId, change.Password, change.OldPassword, change.IsFirstAccess);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("Export2Excel")]
        public async Task<IActionResult> Export2Excel([FromBody] UserFilterDTO filter)
        {
            try
            {
                var result = await _userBO.Export2Excel(filter);
                return File(result.Memory, result.FileExtension, result.FileName);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao exportar o excel. Ex: {ex.Message}");
            }
        }

        [HttpGet("SendEmailTeste")]
        public async Task<IActionResult> SendEmailTeste(string email)
        {
            try
            {
                var result = await _userBO.SendEmail(email);
                return Ok(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao verificar e-mail existente!");
            }
        }
    }
}
