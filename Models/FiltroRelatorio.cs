using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciamentoHoras.Models
{
    public class FiltroRelatorio
    {
        public string? Matricula { get; set; }
        public string? Projeto { get; set; }
        public int? Tipo { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
    }
}