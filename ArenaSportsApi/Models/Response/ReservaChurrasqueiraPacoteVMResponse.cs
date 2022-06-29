using System;

public class ReservaChurrasqueiraPacoteVMResponse
{
    public long ChurrasqueiraPacoteId { get; set; }
    public string Descricao { get; set; }
    public float Valor { get; set; }
    public bool Ativo { get; set; }
    public DateTime? DataExclusao { get; set; }
    public long? UsuarioExclusaoId { get; set; }
}