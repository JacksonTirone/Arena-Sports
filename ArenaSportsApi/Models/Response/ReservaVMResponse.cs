public class ReservaVMResponse
{
    public long ReservaId { get; set; }
    public float ValorTotal { get; set; }

    public ReservaClienteVMResponse Cliente { get; set; }
    public ReservaChurrasqueiraPacoteVMResponse ChurrasqueiraPacote { get; set; }

    public string NomeCliente { get; set; }
    public string Opcionais { get; set; }
    public int? ChurrasqueiraId { get; set; }
    public string ChurrasqueiraNome { get; set; }
}