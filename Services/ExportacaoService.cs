using OfficeOpenXml;
using GerenciamentoHoras.Models;
using ClosedXML.Excel;

namespace GerenciamentoHoras.Services
{

    public class ExportacaoService : IExportacaoService
    {
        public async Task<byte[]> ExportarParaExcelAsync(IEnumerable<RegistroHoras> registros, string nomeArquivo = "RegistrosHoras")
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Registros de Horas");

            var cabecalhos = new[] { "ID", "Nome", "Matricula", "EXT", "Data", "Quantidade de Horas", "Tipo", "Data de Criação" };
            for (int i = 0; i < cabecalhos.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = cabecalhos[i];
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
            }

            int row = 2;
            foreach (var registro in registros)
            {
                worksheet.Cell(row, 1).Value = registro.Id;
                worksheet.Cell(row, 2).Value = registro.Nome?.ToString() ?? "";
                worksheet.Cell(row, 3).Value = registro.Matricula;
                worksheet.Cell(row, 4).Value = registro.EXT;
                worksheet.Cell(row, 5).Value = registro.Data.ToString("dd/MM/yyyy");
                worksheet.Cell(row, 6).Value = registro.QuantidadeHoras;
                worksheet.Cell(row, 7).Value = GetDisplayName(registro.Tipo);
                worksheet.Cell(row, 8).Value = registro.DataCriacao.ToString("dd/MM/yyyy HH:mm");
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return await Task.FromResult(stream.ToArray());
        }

        public async Task<byte[]> ExportarRelatorioAsync(IEnumerable<RegistroHoras> registros, Dictionary<TipoRegistro, decimal> totalPorTipo, string nomeArquivo = "RelatorioHoras")
        {
            using var workbook = new XLWorkbook();

            var resumoSheet = workbook.Worksheets.Add("Resumo");
            resumoSheet.Cell(1, 1).Value = "Resumo de Horas por Tipo";
            resumoSheet.Cell(1, 1).Style.Font.Bold = true;
            resumoSheet.Cell(1, 1).Style.Font.FontSize = 16;

            resumoSheet.Cell(3, 1).Value = "Tipo";
            resumoSheet.Cell(3, 2).Value = "Total de Horas";

            var cabecalho = resumoSheet.Range("A3:B3");
            cabecalho.Style.Font.Bold = true;
            cabecalho.Style.Fill.BackgroundColor = XLColor.LightGray;

            int row = 4;
            foreach (var item in totalPorTipo)
            {
                resumoSheet.Cell(row, 1).Value = GetDisplayName(item.Key);
                resumoSheet.Cell(row, 2).Value = item.Value;
                row++;
            }

            resumoSheet.Cell(row + 1, 1).Value = "TOTAL GERAL";
            resumoSheet.Cell(row + 1, 1).Style.Font.Bold = true;
            resumoSheet.Cell(row + 1, 2).Value = totalPorTipo.Values.Sum();
            resumoSheet.Cell(row + 1, 2).Style.Font.Bold = true;

            resumoSheet.Columns().AdjustToContents();

            var detalhesSheet = workbook.Worksheets.Add("Detalhes");

            var cabecalhos = new[] { "ID", "Matrícula", "Nome", "Projeto", "Data", "Quantidade de Horas", "Tipo", "Data de Criação" };
            for (int i = 0; i < cabecalhos.Length; i++)
            {
                detalhesSheet.Cell(1, i + 1).Value = cabecalhos[i];
                detalhesSheet.Cell(1, i + 1).Style.Font.Bold = true;
                detalhesSheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
            }

            int detalhesRow = 2;
            foreach (var registro in registros)
            {
                detalhesSheet.Cell(detalhesRow, 1).Value = registro.Id;
                detalhesSheet.Cell(detalhesRow, 2).Value = registro.Nome?.ToString() ?? "";
                detalhesSheet.Cell(detalhesRow, 3).Value = registro.Matricula;
                detalhesSheet.Cell(detalhesRow, 4).Value = registro.EXT;
                detalhesSheet.Cell(detalhesRow, 5).Value = registro.Data.ToString("dd/MM/yyyy");
                detalhesSheet.Cell(detalhesRow, 6).Value = registro.QuantidadeHoras;
                detalhesSheet.Cell(detalhesRow, 7).Value = GetDisplayName(registro.Tipo);
                detalhesSheet.Cell(detalhesRow, 8).Value = registro.DataCriacao.ToString("dd/MM/yyyy HH:mm");
                detalhesRow++;
            }

            detalhesSheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return await Task.FromResult(stream.ToArray());
        }

        private string GetDisplayName(TipoRegistro tipo)
        {
            return tipo switch
            {
                TipoRegistro.Normal => "Normal",
                TipoRegistro.Atestado => "Atestado",
                TipoRegistro.Ferias => "Férias",
                _ => tipo.ToString()
            };
        }
    }
}

