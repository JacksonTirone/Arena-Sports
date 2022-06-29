using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArenaSportsApi.Models
{
    public class Churrasqueira
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ChurrasqueiraId { get; set; }
        public string Descricao { get; set; }
        public string DescricaoItens { get; set; }
        public bool Ativo { get; set; }
        public long? UsuarioExclusaoId { get; set; }
        public DateTime? DataExclusao { get; set; }

        [ForeignKey("UsuarioExclusaoId")]
        public Usuario UsuarioExclusao { get; set; }
    }
}
