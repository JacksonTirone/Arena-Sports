using System;
using System.Collections.Generic;

public class QuadraHorariosVMResponse
{
    public int QuadraId { get; set; }
    public string Descricao { get; set; }
    public int PisoId { get; set; }
    public int EsporteId { get; set; }
    public bool Coberta { get; set; }
    public bool Status { get; set; }

    public List<HorarioVMResponse> Horarios { get; set; }

    public QuadraHorariosVMResponse()
    {
        Horarios = new List<HorarioVMResponse>();
    }
}

public class HorarioVMResponse
{
    public int QuadraConfiguracaoHorarioId { get; set; }
    public int QuadraId { get; set; }
    public int DiaSemana { get; set; }
    public string TimeInicio { get; set; }
    public string TimeFim { get; set; }
    public string Duracao { get; set; }
    public float Valor { get; set; }
    public bool Disponivel { get; set; }
    public bool PodeEditarCancelar { get; set; }
    public ReservaVMResponse Reserva { get; set; }
}