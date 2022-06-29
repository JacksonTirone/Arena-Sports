using System;

public class UsuarioRegisterVMRequest
{
    public string Nome { get; set; }
    public string CPF { get; set; }
    public DateTime DataNascimento { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}