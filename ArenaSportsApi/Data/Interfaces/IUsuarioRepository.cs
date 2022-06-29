using System.Collections.Generic;
using System.Threading.Tasks;
using ArenaSportsApi.Models;

public interface IUsuarioRepository
{
    Task<Usuario> CadastrarUsuario(Usuario user);
    Task<List<Usuario>> GetTop10Clientes(string cpfEmail);
}