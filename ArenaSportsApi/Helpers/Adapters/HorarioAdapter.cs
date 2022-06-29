
using System;
using System.Collections.Generic;
using System.Linq;
using ArenaSportsApi.Models;

public static class HorarioAdapter
{
    public static HorarioVMResponse ModelToViewModel(QuadraConfiguracaoHorario configHora, DateTime data, TimeSpan inicio, List<ReservaQuadra> reservaQuadraDia, List<ReservaQuadra> reservaQuadraProximoDia)
    {
        var reservaQuadra = reservaQuadraDia.FirstOrDefault(x => x.TimeInicio == inicio);
        if (inicio.Days >= 1)
            reservaQuadra = reservaQuadraProximoDia.FirstOrDefault(x => x.TimeInicio == inicio.Add(new TimeSpan(-1, 0, 0, 0)));

        var quadraResponse = new HorarioVMResponse
        {
            QuadraConfiguracaoHorarioId = configHora.QuadraConfiguracaoHorarioId,
            QuadraId = configHora.QuadraId,
            Duracao = configHora.Duracao.ToString(@"hh\:mm"),
            DiaSemana = configHora.DiaSemana,
            TimeInicio = inicio.ToString(@"hh\:mm"),
            TimeFim = inicio.Add(configHora.Duracao).ToString(@"hh\:mm"),
            Valor = configHora.Valor,
            Disponivel = reservaQuadra == null && (data.Date > DateTime.Now.Date || (data.Date == DateTime.Now.Date && inicio > DateTime.Now.TimeOfDay)),
            Reserva = reservaQuadra != null ? ReservaAdapter.ModelToViewModel(reservaQuadra) : null,
            PodeEditarCancelar = reservaQuadra != null && (data.Date > DateTime.Now.Date || (data.Date == DateTime.Now.Date && inicio > DateTime.Now.TimeOfDay))
        };

        return quadraResponse;
    }
}