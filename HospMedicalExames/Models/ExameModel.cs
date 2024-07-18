using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HospMedicalExames.Models

{

    public class ExameModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe o nome do exame ")]
        [MaxLength(100)]
        public string NomeExame { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Observacoes { get; set; } = string.Empty;

        [ForeignKey("TipoExame")]
        public int TipoExameId { get; set; }

        public DateTime DataDeNascimento { get; set; } // Nova propriedade

    }
}
