using System;

public class ConfigHorarioQuadraVMRequest
{
        public int QuadraId { get; set; }
        public int DiaSemana { get; set; }
        public TimeSpan TimeInicio { get; set; }
        public TimeSpan TimeFim { get; set; }
        public TimeSpan Duracao { get; set; }
        public float Valor { get; set; }
}