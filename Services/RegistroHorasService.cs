using Microsoft.EntityFrameworkCore;
using GerenciamentoHoras.Data;
using GerenciamentoHoras.Models;

namespace GerenciamentoHoras.Services
{
    public class RegistroHorasService : IRegistroHorasService
    {
        private readonly ApplicationDbContext _context;

        public RegistroHorasService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RegistroHoras>> GetAllAsync()
        {
            return await _context.RegistrosHoras
                .OrderByDescending(r => r.Data)
                .ThenByDescending(r => r.DataCriacao)
                .ToListAsync();
        }

        public async Task<RegistroHoras?> GetByIdAsync(int id)
        {
            return await _context.RegistrosHoras.FindAsync(id);
        }

        public async Task<RegistroHoras> CreateAsync(RegistroHoras registro)
        {
            registro.DataCriacao = DateTime.Now;
            _context.RegistrosHoras.Add(registro);
            await _context.SaveChangesAsync();
            return registro;
        }

        public async Task<RegistroHoras> UpdateAsync(RegistroHoras registro)
        {
            var existingRegistro = await _context.RegistrosHoras.FindAsync(registro.Id);

            if (existingRegistro == null)
                throw new Exception("Registro não encontrado.");

            existingRegistro.Matricula = registro.Matricula;
            existingRegistro.EXT = registro.EXT;
            existingRegistro.Data = registro.Data;
            existingRegistro.QuantidadeHoras = registro.QuantidadeHoras;
            existingRegistro.Tipo = registro.Tipo;

            await _context.SaveChangesAsync();
            return existingRegistro;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var registro = await _context.RegistrosHoras.FindAsync(id);
            if (registro == null)
                return false;

            _context.RegistrosHoras.Remove(registro);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<RegistroHoras>> GetByMatriculaAsync(string matricula)
        {
            return await _context.RegistrosHoras
                .Where(r => r.Matricula == matricula)
                .OrderByDescending(r => r.Data)
                .ToListAsync();
        }

        public async Task<IEnumerable<RegistroHoras>> GetByProjetoAsync(string projeto)
        {
            return await _context.RegistrosHoras
                .Where(r => r.EXT == projeto)
                .OrderByDescending(r => r.Data)
                .ToListAsync();
        }

        public async Task<IEnumerable<RegistroHoras>> GetByTipoAsync(TipoRegistro tipo)
        {
            return await _context.RegistrosHoras
                .Where(r => r.Tipo == tipo)
                .OrderByDescending(r => r.Data)
                .ToListAsync();
        }

        public async Task<IEnumerable<RegistroHoras>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await _context.RegistrosHoras
                .Where(r => r.Data >= dataInicio && r.Data <= dataFim)
                .OrderByDescending(r => r.Data)
                .ToListAsync();
        }

        public async Task<IEnumerable<RegistroHoras>> GetRelatorioAsync(string? matricula = null, string? projeto = null, TipoRegistro? tipo = null, DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            var query = _context.RegistrosHoras.AsQueryable();

            if (!string.IsNullOrEmpty(matricula))
                query = query.Where(r => r.Matricula == matricula);

            if (!string.IsNullOrEmpty(projeto))
                query = query.Where(r => r.EXT == projeto);

            if (tipo.HasValue)
                query = query.Where(r => r.Tipo == tipo.Value);

            if (dataInicio.HasValue)
                query = query.Where(r => r.Data >= dataInicio.Value);

            if (dataFim.HasValue)
                query = query.Where(r => r.Data <= dataFim.Value);

            return await query
                .OrderByDescending(r => r.Data)
                .ThenByDescending(r => r.DataCriacao)
                .ToListAsync();
        }

        public async Task<Dictionary<TipoRegistro, decimal>> GetTotalHorasPorTipoAsync(string? matricula = null, string? projeto = null, DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            var query = _context.RegistrosHoras.AsQueryable();

            if (!string.IsNullOrEmpty(matricula))
                query = query.Where(r => r.Matricula == matricula);

            if (!string.IsNullOrEmpty(projeto))
                query = query.Where(r => r.EXT == projeto);

            if (dataInicio.HasValue)
                query = query.Where(r => r.Data >= dataInicio.Value);

            if (dataFim.HasValue)
                query = query.Where(r => r.Data <= dataFim.Value);

            var resultado = await query
                .GroupBy(r => r.Tipo)
                .Select(g => new { Tipo = g.Key, Total = g.Sum(r => r.QuantidadeHoras) })
                .ToListAsync();

            var dictionary = new Dictionary<TipoRegistro, decimal>();

            // Inicializar todos os tipos com 0
            foreach (TipoRegistro tipoEnum in Enum.GetValues<TipoRegistro>())
            {
                dictionary[tipoEnum] = 0;
            }

            // Preencher com os valores reais
            foreach (var item in resultado)
            {
                dictionary[item.Tipo] = item.Total;
            }

            return dictionary;
        }
    }
}

