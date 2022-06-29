using System.Linq;
using ArenaSportsApi.Models;

public static class ReservaAdapter
{
    public static ReservaVMResponse ModelToViewModel(ReservaQuadra reservaQuadra)
    {
        var reservaChurrasqueira = reservaQuadra.Reserva.Churrasqueiras.FirstOrDefault();
        var reservaOpcionais = reservaQuadra.Opcionais.ToList();

        var reservaResponse = new ReservaVMResponse
        {
            ReservaId = reservaQuadra.Reserva.ReservaId,
            ValorTotal = reservaQuadra.Reserva.ValorTotal,
            NomeCliente = reservaQuadra.Reserva.Cliente.Nome,
            Opcionais = string.Join(", ", reservaOpcionais.Select(x => x.QuadraItemOpcional.Descricao))
        };

        if (reservaChurrasqueira != null)
        {
            reservaResponse.ChurrasqueiraNome = reservaChurrasqueira.Churrasqueira.Descricao;
            reservaResponse.ChurrasqueiraId = reservaChurrasqueira.ChurrasqueiraId;
            reservaResponse.ChurrasqueiraPacote = ChurrasqueiraPacoteAdapter.ModelToViewModel(reservaChurrasqueira.Pacote);
        }

        return reservaResponse;
    }
}