public class LoginVMResponse
{
    public string Token { get; set; }
    public UsuarioLogado UsuarioLogado { get; set; }

    public LoginVMResponse()
    {
        UsuarioLogado = new UsuarioLogado();
    }
}