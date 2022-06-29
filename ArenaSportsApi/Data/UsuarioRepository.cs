using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArenaSportsApi.Models;
using Microsoft.EntityFrameworkCore;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly DataContext _context;
    public UsuarioRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Usuario> CadastrarUsuario(Usuario user)
    {
        if (user.UsuarioId == 0)
            await _context.Usuario.AddAsync(user);
        else
            _context.Entry(user).State = EntityState.Modified;

        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<List<Usuario>> GetTop10Clientes(string cpfNome)
    {
        return await _context.Usuario
            .Where(x => x.TipoUsuarioId == (int)eTipoUsuario.Cliente && 
                        (cpfNome == null || cpfNome == "" || (x.CPF.Contains(cpfNome) || x.Nome.ToLower().Contains(cpfNome.ToLower()))))
            .Take(10)
            .AsNoTracking()
            .ToListAsync();
    }
}