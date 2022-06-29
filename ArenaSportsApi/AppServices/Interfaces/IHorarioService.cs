using System.Collections.Generic;
using System.Threading.Tasks;
using ArenaSportsApi.Models;

public interface IHorarioService
{
     Task<EmptyResponse> ConfigHorarioQuadra(UsuarioLogado userLogado, ConfigHorarioQuadraVMRequest requestConfigHoraQuadra);
     Task<ServiceResponse<List<QuadraHorariosVMResponse>>> GetHorariosQuadras(HorariosQuadrasVMRequest requestQuadraHoraRequest);
     Task<ServiceResponse<List<QuadraConfiguracaoHorario>>> GetConfigHorario(UsuarioLogado userLogado, ConfiguracaoHorarioVMRequest request);
     Task<EmptyResponse> DeleteConfigHorario(UsuarioLogado userLogado, int quadraConfigHoraId);
}