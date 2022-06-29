using System;
using System.Collections.Generic;

public class RealizarReservaVMRequest
{
        public int QuadraConfiguracaoHorarioId { get; set; }
        public long? UsuarioId { get; set; }
        public int QuadraId { get; set; }
        public DateTime DataReserva { get; set; }
        public TimeSpan TimeInicio { get; set; }
        public int? ChurrasqueiraId { get; set; }
        public int? ChurrasqueiraPacoteId { get; set; }
        public List<int> QuadraItemOpcionalIds { get; set; }
}