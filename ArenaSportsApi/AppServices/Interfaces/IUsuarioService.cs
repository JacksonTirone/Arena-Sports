using System.Collections.Generic;
using System.Threading.Tasks;
using ArenaSportsApi.Models;

public interface IUsuarioService
{
     Task<EmptyResponse> Register(Usuario user, string password);
     Task<EmptyResponse> ConfirmarUsuario(string token);
     Task<ServiceResponse<LoginVMResponse>> Login(string email, string password);
     Task<EmptyResponse> EsqueciEmail(string cpf);
     Task<EmptyResponse> EsqueciSenha(string email);
     Task<ServiceResponse<ClienteVMResponse>> CadastroSimples(Usuario user);
     Task<List<ClienteVMResponse>> GetTop10Clientes(string cpfNome);
}