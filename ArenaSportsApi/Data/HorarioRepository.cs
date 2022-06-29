using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArenaSportsApi.Models;
using Microsoft.EntityFrameworkCore;

public class HorarioRepository : IHorarioRepository
{
    private readonly DataContext _context;
    public HorarioRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<QuadraConfiguracaoHorario> GetConfigQuadraById(int configHoraQuadraId)
    {
        return await _context.QuadraConfiguracaoHorario
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.QuadraConfiguracaoHorarioId == configHoraQuadraId);
    }

    public async Task<int> AddConfigQuadra(QuadraConfiguracaoHorario configHoraQuadra)
    {
        await _context.QuadraConfiguracaoHorario.AddAsync(configHoraQuadra);
        await _context.SaveChangesAsync();

        return configHoraQuadra.QuadraConfiguracaoHorarioId;
    }
    
    public async Task<List<Quadra>> GetQuadrasHorariosByDayOfWeek(int dayOfWeek)
    {
        return await _context.Quadra
            .Include(i => i.ConfigsHorarioQuadra)
            .AsNoTracking()
            .Where(x => x.Status && x.ConfigsHorarioQuadra.Any(y => y.DiaSemana == dayOfWeek) && x.Ativo)
            .ToListAsync();
    }

    public async Task<bool> DeleteConfigHora(QuadraConfiguracaoHorario configHora)
    {
        _context.Entry(configHora).State = EntityState.Deleted;
        return await _context.SaveChangesAsync() > 0;
    }
}
