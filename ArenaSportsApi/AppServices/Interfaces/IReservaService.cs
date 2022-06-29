using System.Threading.Tasks;

public interface IReservaService
{
     Task<EmptyResponse> RealizarReserva(UsuarioLogado usuarioLogado, RealizarReservaVMRequest request);
     Task<EmptyResponse> CancelarReserva(UsuarioLogado userLogado, int reservaId);
}