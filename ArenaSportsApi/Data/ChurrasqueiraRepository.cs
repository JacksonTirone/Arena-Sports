using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArenaSportsApi.Models;
using Microsoft.EntityFrameworkCore;

public class ChurrasqueiraRepository : IChurrasqueiraRepository
{
    private readonly DataContext _context;
    
    public ChurrasqueiraRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Churrasqueira>> Get()
    {
        return await _context.Churrasqueira
            .AsNoTracking()
            .Where(y => y.Ativo)
            .OrderBy(x => x.ChurrasqueiraId)
            .ToListAsync();
    }

    public async Task<List<Churrasqueira>> GetDisponiveis(DateTime data, int turno)
    {
        var idsIndisponiveis = await _context.ReservaChurrasqueira
            .Include(x => x.Reserva)
            .AsNoTracking()
            .Where(x => x.Reserva.DataReserva.Date == data.Date && x.Turno == turno)
            .Select(c => c.ChurrasqueiraId)
            .ToListAsync();

        return await _context.Churrasqueira
            .AsNoTracking()
            .Where(y => y.Ativo && !idsIndisponiveis.Contains(y.ChurrasqueiraId))
            .OrderBy(x => x.ChurrasqueiraId)
            .ToListAsync();
    }

    public async Task<List<ChurrasqueiraPacote>> GetPacotes()
    {
        return await _context.ChurrasqueiraPacote
            .AsNoTracking()
            .Where(y => y.Ativo)
            .OrderBy(x => x.ChurrasqueiraPacoteId)
            .ToListAsync();
    }

    public async Task<Churrasqueira> GetChurrasqueiraById(int churrasqueiraId)
    {
        return await _context.Churrasqueira
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ChurrasqueiraId == churrasqueiraId && x.Ativo);
    }

    public async Task<ChurrasqueiraPacote> GetChurrasqueiraPacoteById(int pacoteId)
    {
        return await _context.ChurrasqueiraPacote
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ChurrasqueiraPacoteId == pacoteId && x.Ativo);
    }

    public async Task<bool> CadastrarChurrasqueira(Churrasqueira churrasqueira)
    {
        await _context.Churrasqueira.AddAsync(churrasqueira);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateChurrasqueira(Churrasqueira churrasqueira)
    {
        _context.Entry(churrasqueira).State = EntityState.Modified;
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> CadastrarPacote(ChurrasqueiraPacote itemPacote)
    {
        await _context.ChurrasqueiraPacote.AddAsync(itemPacote);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdatePacote(ChurrasqueiraPacote itemPacote)
    {
        _context.Entry(itemPacote).State = EntityState.Modified;
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteChurrasqueira(Churrasqueira churras)
    {
        _context.Entry(churras).State = EntityState.Modified;
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteChurrasqueiraPacote(ChurrasqueiraPacote itemPacote)
    {
        _context.Entry(itemPacote).State = EntityState.Modified;
        return await _context.SaveChangesAsync() > 0;
    }
}
