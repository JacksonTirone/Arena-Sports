using System.Threading.Tasks;
using ArenaSportsApi.Models;

public interface IAuthRepository
{
    Task<Usuario> GetUserByEmail(string email);
    Task<Usuario> GetUserByCPF(string cpf);
    Task<bool> EmailExists(string email);
    Task<bool> CPFExists(string cpf);
    Task<UsuarioEmailConfirmacao> GetConfirmacaoUsuario(string token);
    Task<bool> ConfirmarUsuario(UsuarioEmailConfirmacao confirmar);
}