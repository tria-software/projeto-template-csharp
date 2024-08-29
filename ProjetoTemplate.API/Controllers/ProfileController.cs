using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoTemplate.BL.Profile;
using ProjetoTemplate.Domain.DTO;
using ProjetoTemplate.Domain.DTO.Profile;
using System;
using System.Threading.Tasks;

namespace ProjetoTemplate.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class ProfileController : Controller
    {
        private readonly IProfileBO _profileBO;

        public ProfileController(IProfileBO profileBO)
        {
            _profileBO = profileBO;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll([FromBody] ProfileFilterDTO filter)
        {
            try
            {
                var result = await _profileBO.GetAll(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha na busca dos perfis. Ex: {ex.Message}");
            }
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] ProfileDTO category)
        {
            try
            {
                var result = await _profileBO.SaveUpdate(category);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao salvar Perfil. Ex: {ex.Message}");
            }
        }

        [HttpPatch("Update")]
        public async Task<IActionResult> Update([FromBody] ProfileDTO category)
        {
            try
            {
                var result = await _profileBO.SaveUpdate(category);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao atualizar perfil. Ex: {ex.Message}");
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                var result = await _profileBO.GetById(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha na busca da perfil. Ex: {ex.Message}");
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteProfile(long Id)
        {
            try
            {
                var result = await _profileBO.DeleteProfile(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao deletar perfil. Ex: {ex.Message}");
            }
        }

        [HttpPatch("ActivateDisable")]
        public async Task<IActionResult> ActivateDisable([FromBody] ActivateDisableDeleteDTO model)
        {
            try
            {
                var result = await _profileBO.ActivateDisable(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao atualizar Perfil. Ex: {ex.Message}");
            }
        }

        [HttpPost("Export2Excel")]
        public async Task<IActionResult> Export2Excel([FromBody] ProfileFilterDTO filter)
        {
            try
            {
                var result = await _profileBO.Export2Excel(filter);
                return File(result.Memory, result.FileExtension, result.FileName);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao exportar o excel. Ex: {ex.Message}");
            }
        }
    }
}
