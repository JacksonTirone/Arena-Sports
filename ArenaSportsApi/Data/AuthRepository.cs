using System;
using System.Threading.Tasks;
using ArenaSportsApi.Models;
using Microsoft.EntityFrameworkCore;

public class AuthRepository : IAuthRepository
{
    private readonly DataContext _context;
    public AuthRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> EmailExists(string email) => await _context.Usuario.AnyAsync(x => x.Email.ToLower() == email.ToLower());

    public async Task<bool> CPFExists(string cpf) => await _context.Usuario.AnyAsync(x => x.CPF.ToLower() == cpf.ToLower() && x.Email != null && x.PasswordHash != null);

    public async Task<Usuario> GetUserByEmail(string email) => await _context.Usuario
                                                                        .Include(i => i.UsuarioConfirmacoesEmail)
                                                                        .AsNoTracking()
                                                                        .FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));

    public async Task<Usuario> GetUserByCPF(string cpf) => await _context.Usuario
                                                                .AsNoTracking()
                                                                .FirstOrDefaultAsync(x => x.CPF.ToLower().Equals(cpf.Replace("-", string.Empty).Replace(".", string.Empty).ToLower()));

    public async Task<UsuarioEmailConfirmacao> GetConfirmacaoUsuario(string token) => await _context.UsuarioEmailConfirmacao
                                                                                                //.Include(i => i.Usuario)
                                                                                                .AsNoTracking()
                                                                                                .FirstOrDefaultAsync(x => x.Token.Equals(token));

    public async Task<bool> ConfirmarUsuario(UsuarioEmailConfirmacao confirmar)
    {
        confirmar.DataConfirmacao = DateTime.Now;
        confirmar.EmailConfirmado = true;
        
        _context.Entry(confirmar).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        
        return true;
    }
}
