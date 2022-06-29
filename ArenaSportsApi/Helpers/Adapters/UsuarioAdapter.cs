
using ArenaSportsApi.Models;

public static class UsuarioAdapter
{
    public static Usuario ViewModelToModel(UsuarioRegisterVMRequest usuarioRequest)
    {
        var usuario = new Usuario
        {
            CPF = usuarioRequest.CPF.Replace("-", string.Empty).Replace(".", string.Empty),
            DataNascimento = usuarioRequest.DataNascimento,
            Email = usuarioRequest.Email.ToLower(),
            Nome = usuarioRequest.Nome,
            TipoUsuarioId = (int)eTipoUsuario.Cliente
        };

        return usuario;
    }

    public static Usuario ViewModelToModel(CadastroSimplesVMRequest usuarioRequest)
    {
        var usuario = new Usuario
        {
            CPF = usuarioRequest.CPF.Replace("-", string.Empty).Replace(".", string.Empty),
            Nome = usuarioRequest.Nome,
            TipoUsuarioId = (int)eTipoUsuario.Cliente
        };

        return usuario;
    }
}