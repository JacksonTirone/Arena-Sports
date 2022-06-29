using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArenaSportsApi.Models
{
    public class UsuarioEmailConfirmacao
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long UsuarioEmailConfirmacaoId { get; set; }
        public long UsuarioId { get; set; }
        public string Token { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public string Email { get; set; }
        public DateTime? DataConfirmacao { get; set; }
        public bool EmailConfirmado { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }
    }
}
