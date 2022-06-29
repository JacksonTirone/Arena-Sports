
using System;
using System.Collections.Generic;
using System.Linq;
using ArenaSportsApi.Models;

public static class QuadraAdapter
{
    public static QuadraHorariosVMResponse ModelToViewModel(Quadra quadra, DateTime data, List<ReservaQuadra> reservasQuadrasDia, List<ReservaQuadra> reservasQuadrasProximoDia)
    {
        var reservasDestaQuadraDia = reservasQuadrasDia.Where(x => x.QuadraId == quadra.QuadraId).ToList();
        var reservasDestaQuadraProximoDia = reservasQuadrasProximoDia.Where(x => x.QuadraId == quadra.QuadraId).ToList();

        var quadraResponse = new QuadraHorariosVMResponse
        {
            QuadraId = quadra.QuadraId,
            Coberta = quadra.Coberta,
            Descricao = quadra.Descricao,
            EsporteId = quadra.EsporteId,
            PisoId = quadra.PisoId,
            Status = quadra.Status
        };

        var horariosResponse = new List<HorarioVMResponse>();
        foreach (var configHorario in quadra.ConfigsHorarioQuadra.Where(x => x.DiaSemana == new ComumService().GetDayOfWeek(data)).OrderBy(x => x.TimeInicio))
        {
            var horaInicial = configHorario.TimeInicio;
            for (int i = 0; i < 1440 / configHorario.Duracao.TotalMinutes; i++)
            {
                horariosResponse.Add(HorarioAdapter.ModelToViewModel(configHorario, data, horaInicial, reservasDestaQuadraDia, reservasDestaQuadraProximoDia));
                horaInicial = horaInicial.Add(configHorario.Duracao);
                if (horaInicial.Hours == configHorario.TimeFim.Hours && horaInicial.Minutes == configHorario.TimeFim.Minutes)
                    break;
            }
        }
        quadraResponse.Horarios.AddRange(horariosResponse);
        return quadraResponse;
    }

    public static List<QuadraHorariosVMResponse> ModelToViewModel(List<Quadra> quadras, DateTime data, List<ReservaQuadra> reservasQuadrasDia, List<ReservaQuadra> reservasQuadrasProximoDia)
    {
        var quadrasVM = new List<QuadraHorariosVMResponse>();
        
        foreach (var item in quadras)
        {
            quadrasVM.Add(ModelToViewModel(item, data, reservasQuadrasDia, reservasQuadrasProximoDia));
        };

        return quadrasVM;
    }
}