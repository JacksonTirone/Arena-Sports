using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArenaSportsApi.Models
{
    public class ReservaQuadra
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long ReservaQuadraId { get; set; }
        public long ReservaId { get; set; }
        public int QuadraId { get; set; }
        public TimeSpan TimeInicio { get; set; }
        public TimeSpan TimeFim { get; set; }
        public float Valor { get; set; }

        [ForeignKey("ReservaId")]
        public Reserva Reserva { get; set; }

        [ForeignKey("QuadraId")]
        public Quadra Quadra { get; set; }

        public List<ReservaQuadraOpcional> Opcionais { get; set; }

        public ReservaQuadra(){
            Opcionais = new List<ReservaQuadraOpcional>();
        }
    }
}
