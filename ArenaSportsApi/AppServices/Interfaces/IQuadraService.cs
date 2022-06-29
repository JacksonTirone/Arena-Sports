using System.Collections.Generic;
using System.Threading.Tasks;
using ArenaSportsApi.Models;

public interface IQuadraService
{
     Task<ServiceResponse<List<QuadraVMResponse>>> GetAll();
     Task<ServiceResponse<List<QuadraItemOpcional>>> GetAllOpcionais();
     Task<EmptyResponse> CadastrarAtualizarQuadra(UsuarioLogado userLogado, CadastrarQuadraVMRequest requestQuadra);
     Task<EmptyResponse> CadastrarAtualizarItemQuadra(UsuarioLogado userLogado, QuadraItemOpcionalVMRequest requestItem);
     Task<EmptyResponse> DeleteQuadra(UsuarioLogado userLogado, int quadraId);
     Task<EmptyResponse> DeleteOpcionalQuadra(UsuarioLogado userLogado, int opcionalQuadraId);
}