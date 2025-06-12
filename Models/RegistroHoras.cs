using System.ComponentModel.DataAnnotations;

namespace GerenciamentoHoras.Models
{
    public class RegistroHoras
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Matrícula é obrigatória")]
        [Display(Name = "Matrícula")]
        public string Matricula { get; set; } = string.Empty;

        [Required(ErrorMessage = "EXT é obrigatório")]
        [Display(Name = "EXT")]
        public string EXT { get; set; } = string.Empty;

        [Required(ErrorMessage = "Data é obrigatória")]
        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "Quantidade de horas é obrigatória")]
        [Display(Name = "Quantidade de Horas")]
        [Range(0.1, 168, ErrorMessage = "A quantidade de horas deve estar entre 0.1 e 168")]
        public decimal QuantidadeHoras { get; set; }

        [Required(ErrorMessage = "Tipo é obrigatório")]
        [Display(Name = "Tipo")]
        public TipoRegistro Tipo { get; set; }

        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }

    public enum TipoRegistro
    {
        [Display(Name = "Normal")]
        Normal = 1,

        [Display(Name = "Atestado")]
        Atestado = 2,

        [Display(Name = "Férias")]
        Ferias = 3
    }
}

