using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArenaSportsApi.Models;
using Microsoft.EntityFrameworkCore;

public class ReservaRepository : IReservaRepository
{
    private readonly DataContext _context;
    public ReservaRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<ReservaQuadra>> GetReservaQuadrasByData(DateTime data, List<int> quadrasIds)
    {
        return await _context.ReservaQuadra
            .Include(x => x.Quadra)
            .Include(x => x.Opcionais)
            .Include("Opcionais.QuadraItemOpcional")
            .Include(x => x.Reserva)
            .Include(x => x.Reserva.Cliente)
            .Include(x => x.Reserva.Churrasqueiras)
            .Include("Reserva.Churrasqueiras.Churrasqueira")
            .Include("Reserva.Churrasqueiras.Pacote")
            .AsNoTracking()
            .Where(x => x.Reserva.DataReserva.Date == data.Date && quadrasIds.Contains(x.QuadraId) && x.Reserva.StatusId == (int)eStatus.Reservada)
            .ToListAsync();
    }

    public bool VerificaHorarioLivreQuadra(int quadraId, DateTime data, TimeSpan horaInicio)
    {
        return !_context.ReservaQuadra
            .AsNoTracking()
            .Any(x => x.QuadraId == quadraId && x.Reserva.DataReserva == data.Date && horaInicio >= x.TimeInicio && horaInicio < x.TimeFim && x.Reserva.StatusId == (int)eStatus.Reservada);
    }

    public bool VerificaHorarioLivreChurrasqueira(int churrasqueiraId, DateTime data, int turno)
    {
        return !_context.ReservaChurrasqueira
            .AsNoTracking()
            .Any(x => x.ChurrasqueiraId == churrasqueiraId && x.Reserva.DataReserva == data.Date && x.Turno == turno);
    }

    public async Task<long> RealizarReserva(Reserva reserva)
    {
        await _context.Reserva.AddAsync(reserva);
        await _context.SaveChangesAsync();

        return reserva.ReservaId;
    }

    public async Task<Reserva> GetReserva(int id)
    {
        return await _context.Reserva
            .AsNoTracking()
            .Where(x => id == x.ReservaId)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> CancelarReserva(UsuarioLogado userLogado, Reserva reserva)
    {
        reserva.DataCancelamento = DateTime.Now;
        reserva.CancelamentoUsuarioId = userLogado.UsuarioId;
        reserva.StatusId = (int)eStatus.Cancelada;

        _context.Entry(reserva).State = EntityState.Modified;
        return await _context.SaveChangesAsync() > 0;
    }
}