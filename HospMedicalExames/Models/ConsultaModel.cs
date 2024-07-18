using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HospMedicalExames.Models
{
    public class ConsultaModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Infome a data e o horario da consulta")]
        [Display(Name = "Data e Hora")]
        public DateTime DataHora { get; set; } = DateTime.Now.ToLocalTime();


        [ForeignKey("Exame")]
        public int ExameId { get; set; }

        [ForeignKey("Paciente")]
        public int PacienteId { get; set; }

        [ForeignKey("TipoExame")]
        public int TipoExameId { get; set; }

#pragma warning disable CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere declará-lo como anulável.
        public string Protocolo { get; set; }
#pragma warning restore CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere declará-lo como anulável.

    }
}