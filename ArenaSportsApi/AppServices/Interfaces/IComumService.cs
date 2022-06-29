using System;
using ArenaSportsApi.Models;

public interface IComumService
{
     bool ValidarIntervaloHorario(Quadra quadra, int diaSemana, TimeSpan timeInicio, TimeSpan timeFinal);
     int GetDayOfWeek(DateTime data);
}