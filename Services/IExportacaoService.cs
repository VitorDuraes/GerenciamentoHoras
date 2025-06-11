using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GerenciamentoHoras.Models;

namespace GerenciamentoHoras.Services
{
    public interface IExportacaoService
    {
        Task<byte[]> ExportarParaExcelAsync(IEnumerable<RegistroHoras> registros, string nomeArquivo = "RegistrosHoras");
        Task<byte[]> ExportarRelatorioAsync(IEnumerable<RegistroHoras> registros, Dictionary<TipoRegistro, decimal> totalPorTipo, string nomeArquivo = "RelatorioHoras");
    }

}