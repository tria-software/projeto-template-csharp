using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoTemplate.BL.LayoutLot;
using System.Threading.Tasks;
using System;
using ProjetoTemplate.API.Controllers.Base;
using ProjetoTemplate.Domain.DTO.LayoutLot;

namespace ProjetoTemplate.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class LayoutColumnsController : ApiBaseController
    {
        private readonly ILayoutColumnsBO _layoutColumnsBO;

        public LayoutColumnsController(ILayoutColumnsBO layoutColumnsBO)
        {
            _layoutColumnsBO = layoutColumnsBO;
        }

        [HttpGet("GetAll/{table}")]
        public async Task<IActionResult> GetAll(string table)
        {
            try
            {
                var result = await _layoutColumnsBO.GetAll(UserId, table);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha na busca dos Layouts. Ex: {ex.Message}");
            }
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] CreateLayoutDTO lot)
        {
            try
            {
                lot.UserId = UserId;
                var result = await _layoutColumnsBO.SaveUpdate(lot);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao salvar Layout. Ex: {ex.Message}");
            }
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            try
            {
                var result = await _layoutColumnsBO.DeleteLayout(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha ao deletar Layout. Ex: {ex.Message}");
            }
        }
    }
}
