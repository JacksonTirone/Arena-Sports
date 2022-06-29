using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArenaSportsApi.Models
{
    public class Reserva
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long ReservaId { get; set; }
        public long ReservaUsuarioId { get; set; }
        public int StatusId { get; set; }
        public DateTime DataReserva { get; set; }
        public float ValorTotal { get; set; }
        public DateTime DataRequisicao { get; set; }
        public long RequisicaoUsuarioId { get; set; }
        public long? CancelamentoUsuarioId { get; set; }
        public DateTime? DataCancelamento { get; set; }

        [ForeignKey("ReservaUsuarioId")]
        public Usuario Cliente { get; set; }

        [ForeignKey("RequisicaoUsuarioId")]
        public Usuario UsuarioRequisicao { get; set; }

        [ForeignKey("CancelamentoUsuarioId")]
        public Usuario UsuarioCancelamento { get; set; }

        public List<ReservaQuadra> Quadras { get; set; }
        public List<ReservaChurrasqueira> Churrasqueiras { get; set; }

        public Reserva() {
            Quadras = new List<ReservaQuadra>();
            Churrasqueiras = new List<ReservaChurrasqueira>();
        }
    }
}