using System;
using System.Linq;
using ArenaSportsApi.Models;

public class ComumService : IComumService
{
    private readonly DateTime dateToday;

    public ComumService()
    {
        dateToday = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
    }

    public bool ValidarIntervaloHorario(Quadra quadra, int diaSemana, TimeSpan timeInicio, TimeSpan timeFinal)
    {
        var configsHorarios = quadra.ConfigsHorarioQuadra.Where(x => x.DiaSemana == diaSemana).ToList();
        foreach (var item in configsHorarios)
        {
            var dataInicioBD = dateToday + item.TimeInicio;
            var dataFimBD = dateToday + item.TimeFim;
            if (dataInicioBD > dataFimBD)
                dataFimBD = dataFimBD.AddDays(1);

            var dataInicioRequest = dateToday + timeInicio;
            var dataFimRequest = dateToday + timeFinal;
            if (dataInicioRequest > dataFimRequest)
                dataFimRequest = dataFimRequest.AddDays(1);

            if ((dataInicioRequest >= dataInicioBD && dataInicioRequest < dataFimBD) 
                || (dataFimRequest > dataInicioBD && dataFimRequest <= dataFimBD))
                return false;
        }
        return true;
    }

    public int GetDayOfWeek(DateTime data)
    {
        var dayOfWeek = new Int32();
        switch (data.DayOfWeek)
        {
            case DayOfWeek.Sunday:
                dayOfWeek = 0;
                break;
            case DayOfWeek.Monday:
                dayOfWeek = 1;
                break;
            case DayOfWeek.Tuesday:
                dayOfWeek = 2;
                break;
            case DayOfWeek.Wednesday:
                dayOfWeek = 3;
                break;
            case DayOfWeek.Thursday:
                dayOfWeek = 4;
                break;
            case DayOfWeek.Friday:
                dayOfWeek = 5;
                break;
            case DayOfWeek.Saturday:
                dayOfWeek = 6;
                break;
        }

        return dayOfWeek;
    }
}