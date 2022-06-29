using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArenaSportsApi.Models;
using Microsoft.EntityFrameworkCore;

public class QuadraRepository : IQuadraRepository
{
    private readonly DataContext _context;
    public QuadraRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Quadra> GetQuadraById(int quadraId)
    {
        return await _context.Quadra
            .Include(i => i.ConfigsHorarioQuadra)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.QuadraId.Equals(quadraId) && x.Ativo);
    }

    public async Task<QuadraItemOpcional> GetOpcionalQuadra(int opcionalId)
    {
        return await _context.QuadraItemOpcional
            .AsNoTracking()
            .Where(x => opcionalId == x.QuadraItemOpcionalId && x.Ativo)
            .FirstOrDefaultAsync();
    }

    public async Task<List<QuadraItemOpcional>> GetItensOpcionais(List<int> idsItens)
    {
        return await _context.QuadraItemOpcional
            .AsNoTracking()
            .Where(x => idsItens.Contains(x.QuadraItemOpcionalId) && x.Ativo)
            .ToListAsync();
    }

    public async Task<bool> CadastrarQuadra(Quadra quadra)
    {
        await _context.Quadra.AddAsync(quadra);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateQuadra(Quadra quadra)
    {
        _context.Entry(quadra).State = EntityState.Modified;
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> CadastrarItemQuadra(QuadraItemOpcional item)
    {
        await _context.QuadraItemOpcional.AddAsync(item);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateItemQuadra(QuadraItemOpcional item)
    {
        _context.Entry(item).State = EntityState.Modified;
        return await _context.SaveChangesAsync() > 0;
    }
    
    public async Task<List<Quadra>> GetAll()
    {
        return await _context.Quadra
            .AsNoTracking()
            .Where(x => x.Ativo)
            .OrderBy(x => x.QuadraId)
            .ToListAsync();
    }
    
    public async Task<List<QuadraItemOpcional>> GetOpcionais()
    {
        return await _context.QuadraItemOpcional
            .AsNoTracking()
            .Where(x => x.Ativo)
            .OrderBy(x => x.QuadraItemOpcionalId)
            .ToListAsync();
    }

    public async Task<List<QuadraConfiguracaoHorario>> GetConfigHorario(int quadraId, int diaSemana)
    {
        return await _context.QuadraConfiguracaoHorario
            .AsNoTracking()
            .Where(x => x.DiaSemana == diaSemana && x.QuadraId == quadraId)
            .ToListAsync();
    }

    public async Task<bool> DeleteQuadra(Quadra quadra)
    {
        _context.Entry(quadra).State = EntityState.Modified;
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteOpcionalQuadra(QuadraItemOpcional opcionalQuadra)
    {
        _context.Entry(opcionalQuadra).State = EntityState.Modified;
        return await _context.SaveChangesAsync() > 0;
    }
}
