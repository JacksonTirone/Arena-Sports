using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArenaSportsApi.Models;

public interface IReservaRepository
{
    Task<List<ReservaQuadra>> GetReservaQuadrasByData(DateTime data, List<int> quadraIds);
    bool VerificaHorarioLivreQuadra(int quadraId, DateTime data, TimeSpan horaInicio);
    bool VerificaHorarioLivreChurrasqueira(int churrasqueiraId, DateTime data, int turno);
    Task<long> RealizarReserva(Reserva reserva);
    Task<Reserva> GetReserva(int id);
    Task<bool> CancelarReserva(UsuarioLogado userLogado, Reserva reservaId);
}