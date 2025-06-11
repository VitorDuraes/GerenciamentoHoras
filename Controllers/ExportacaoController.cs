using Microsoft.AspNetCore.Mvc;
using GerenciamentoHoras.Services;
using GerenciamentoHoras.Models;

namespace GerenciamentoHoras.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExportacaoController : ControllerBase
    {
        private readonly IRegistroHorasService _registroService;
        private readonly IExportacaoService _exportacaoService;

        public ExportacaoController(IRegistroHorasService registroService, IExportacaoService exportacaoService)
        {
            _registroService = registroService;
            _exportacaoService = exportacaoService;
        }

        [HttpGet("excel")]
        public async Task<IActionResult> ExportarTodosParaExcel()
        {
            try
            {
                var registros = await _registroService.GetAllAsync();
                var arquivo = await _exportacaoService.ExportarParaExcelAsync(registros, "RegistrosHoras");

                var nomeArquivo = $"RegistrosHoras_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                return File(arquivo, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nomeArquivo);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao exportar dados: {ex.Message}");
            }
        }

        [HttpPost("relatorio")]
        public async Task<IActionResult> ExportarRelatorio([FromBody] FiltroRelatorio filtro)
        {
            try
            {
                TipoRegistro? tipo = null;
                if (filtro.Tipo.HasValue)
                {
                    tipo = (TipoRegistro)filtro.Tipo.Value;
                }

                var registros = await _registroService.GetRelatorioAsync(
                    filtro.Matricula,
                    filtro.Projeto,
                    tipo,
                    filtro.DataInicio,
                    filtro.DataFim);

                var totalPorTipo = await _registroService.GetTotalHorasPorTipoAsync(
                    filtro.Matricula,
                    filtro.Projeto,
                    filtro.DataInicio,
                    filtro.DataFim);

                var arquivo = await _exportacaoService.ExportarRelatorioAsync(registros, totalPorTipo, "RelatorioHoras");

                var nomeArquivo = $"RelatorioHoras_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                return File(arquivo, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nomeArquivo);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao exportar relatório: {ex.Message}");
            }
        }
    }


}

