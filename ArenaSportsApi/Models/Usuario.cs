using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArenaSportsApi.Models
{
    public class Usuario
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long UsuarioId { get; set; }
        public int TipoUsuarioId { get; set; }
        public string CPF { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public List<UsuarioEmailConfirmacao> UsuarioConfirmacoesEmail { get; set; }

        public Usuario()
        {
            UsuarioConfirmacoesEmail = new List<UsuarioEmailConfirmacao>();
        }

        public void AtualizarPrimeiroCadastro(Usuario user)
        {
            UsuarioId = user.UsuarioId;
            Nome = user.Nome;
            DataNascimento = user.DataNascimento;
            Email = user.Email;
        }
    }
}
