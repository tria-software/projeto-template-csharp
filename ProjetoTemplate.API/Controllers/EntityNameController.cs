using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoTemplate.BL;
using ProjetoTemplate.Domain.DTO;
using ProjetoTemplate.Domain.DTO.EntityName;

namespace ProjetoTemplate.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class EntityNameController : ControllerBase
    {
        private readonly IEntityNameBO _entityNameBO;

        public EntityNameController(IEntityName entityName)
        {
            _entityNameBO = entityName;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll([FromBody] EntityNameFilterDTO filter)
        {
            try
            {
                var result = await _entityNameBO.GetAll(filter);
                return Ok(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha na busca dos EntityNames.");
            }
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] Domain.DTO.EntityName.EntityNameDTO entityName)
        {
            try
            {
                var result = await _entityNameBO.SaveUpdate(entityName);
                return Ok(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao salvar/atualizar EntityName.");
            }
        }

        [HttpPatch("Update")]
        public async Task<IActionResult> Update([FromBody] Domain.DTO.EntityName.EntityNameDTO entityName)
        {
            try
            {
                var result = await _entityNameBO.SaveUpdate(entityName);
                return Ok(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao salvar/atualizar EntityName.");
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                var result = await _entityNameBO.GetById(Id);
                return Ok(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha na busca do EntityName.");
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(long Id)
        {
            try
            {
                var result = await _entityNameBO.Delete(Id);
                return Ok(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao deletar EntityName");
            }
        }

        [HttpPatch("ActivateDisable")]
        public async Task<IActionResult> ActivateDisable([FromBody] ActivateDisableDeleteDTO model)
        {
            try
            {
                var result = await _entityNameBO.ActivateDisable(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao atualizar EntityName. Ex: {ex.Message}");
            }
        }

        [HttpPost("Export2Excel")]
        public async Task<IActionResult> Export2Excel([FromBody] EntityNameFilterDTO filter)
        {
            try
            {
                var result = await _entityNameBO.Export2Excel(filter);
                return File(result.Memory, result.FileExtension, result.FileName);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao exportar o excel. Ex: {ex.Message}");
            }
        }
    }
}
