using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArenaSportsApi.Models
{
    public class QuadraConfiguracaoHorario
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int QuadraConfiguracaoHorarioId { get; set; }
        public int QuadraId { get; set; }
        public int DiaSemana { get; set; }
        public TimeSpan TimeInicio { get; set; }
        public TimeSpan TimeFim { get; set; }
        public TimeSpan Duracao { get; set; }
        public float Valor { get; set; }

        [ForeignKey("QuadraId")]
        public Quadra Quadra { get; set; }
    }
}
