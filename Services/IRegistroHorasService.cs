using GerenciamentoHoras.Models;

namespace GerenciamentoHoras.Services
{
    public interface IRegistroHorasService
    {
        Task<IEnumerable<RegistroHoras>> GetAllAsync();
        Task<RegistroHoras?> GetByIdAsync(int id);
        Task<RegistroHoras> CreateNameAsync(RegistroHoras registroHoras);
        Task<RegistroHoras> CreateAsync(RegistroHoras registro);
        Task<RegistroHoras> UpdateAsync(RegistroHoras registro);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<RegistroHoras>> GetByMatriculaAsync(string matricula);
        Task<IEnumerable<RegistroHoras>> GetByProjetoAsync(string projeto);
        Task<IEnumerable<RegistroHoras>> GetByTipoAsync(TipoRegistro tipo);
        Task<IEnumerable<RegistroHoras>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<RegistroHoras>> GetRelatorioAsync(string? matricula = null, string? nome = null, string? projeto = null, TipoRegistro? tipo = null, DateTime? dataInicio = null, DateTime? dataFim = null);
        Task<Dictionary<TipoRegistro, decimal>> GetTotalHorasPorTipoAsync(string? matricula = null, string? nome = null, string? projeto = null, DateTime? dataInicio = null, DateTime? dataFim = null);
    }
}

