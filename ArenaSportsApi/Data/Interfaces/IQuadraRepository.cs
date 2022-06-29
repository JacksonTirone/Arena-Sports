using System.Collections.Generic;
using System.Threading.Tasks;
using ArenaSportsApi.Models;

public interface IQuadraRepository
{
    Task<Quadra> GetQuadraById(int quadraId);
    Task<QuadraItemOpcional> GetOpcionalQuadra(int opcionalQuadraId);
    Task<List<QuadraItemOpcional>> GetItensOpcionais(List<int> idsItens);
    Task<bool> CadastrarQuadra(Quadra quadra);
    Task<bool> UpdateQuadra(Quadra quadra);
    Task<bool> CadastrarItemQuadra(QuadraItemOpcional itemOpcional);
    Task<bool> UpdateItemQuadra(QuadraItemOpcional itemOpcional);
    Task<List<Quadra>> GetAll();
    Task<List<QuadraItemOpcional>> GetOpcionais();
    Task<List<QuadraConfiguracaoHorario>> GetConfigHorario(int quadraId, int diaSemana);
    Task<bool> DeleteQuadra(Quadra quadra);
    Task<bool> DeleteOpcionalQuadra(QuadraItemOpcional opcionalQuadra);
}