
using ArenaSportsApi.Models;

public static class ChurrasqueiraPacoteAdapter
{
    public static ReservaChurrasqueiraPacoteVMResponse ModelToViewModel(ChurrasqueiraPacote pacote)
    {
        var pacoteVM = new ReservaChurrasqueiraPacoteVMResponse
        {
            ChurrasqueiraPacoteId = pacote.ChurrasqueiraPacoteId,
            Descricao = pacote.Descricao,
            Valor = pacote.Valor,
            Ativo = pacote.Ativo,
            DataExclusao = pacote.DataExclusao,
            UsuarioExclusaoId = pacote.UsuarioExclusaoId
        };

        return pacoteVM;
    }
}