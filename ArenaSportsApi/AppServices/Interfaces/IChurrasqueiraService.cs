using System.Collections.Generic;
using System.Threading.Tasks;
using ArenaSportsApi.Models;

public interface IChurrasqueiraService
{
     Task<ServiceResponse<List<Churrasqueira>>> GetAll();
     Task<ServiceResponse<List<Churrasqueira>>> GetAllDisponiveis(ChurrasqueirasDisponiveisVMRequest resquest);
     Task<ServiceResponse<List<ChurrasqueiraPacote>>> GetAllPacote();
     Task<EmptyResponse> CadastrarAtualizarChurrasqueira(UsuarioLogado userLogado, ChurrasqueiraVMRequest requestChurras);
     Task<EmptyResponse> CadastrarAtualizarPacote(UsuarioLogado userLogado, ChurrasqueiraPacoteVMRequest requestChurrasPacote);
     Task<EmptyResponse> DeleteChurrasqueira(UsuarioLogado userLogado, int id);
     Task<EmptyResponse> DeletePacoteChurrasqueira(UsuarioLogado userLogado, int id);
}