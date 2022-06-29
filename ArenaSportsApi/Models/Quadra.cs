using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArenaSportsApi.Models
{
    public class Quadra
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int QuadraId { get; set; }
        public string Descricao { get; set; }
        public int PisoId { get; set; }
        public int EsporteId { get; set; }
        public bool Coberta { get; set; }
        public bool Status { get; set; }
        public bool Ativo { get; set; }
        public long? UsuarioExclusaoId { get; set; }
        public DateTime? DataExclusao { get; set; }


        [ForeignKey("UsuarioExclusaoId")]
        public Usuario Usuario { get; set; }
        
        public List<QuadraConfiguracaoHorario> ConfigsHorarioQuadra { get; set; }

        public Quadra()
        {
            ConfigsHorarioQuadra = new List<QuadraConfiguracaoHorario>();
        }
    }
}