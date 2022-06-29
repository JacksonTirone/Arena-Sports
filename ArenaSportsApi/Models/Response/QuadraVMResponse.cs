namespace ArenaSportsApi.Models
{
    public class QuadraVMResponse
    {
        public int QuadraId { get; set; }
        public string Descricao { get; set; }
        public int PisoId { get; set; }
        public int EsporteId { get; set; }
        public bool Coberta { get; set; }
        public bool Status { get; set; }
    }
}