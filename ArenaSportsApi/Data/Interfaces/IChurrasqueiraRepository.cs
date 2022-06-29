using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArenaSportsApi.Models;

public interface IChurrasqueiraRepository
{
    Task<List<Churrasqueira>> Get();
    Task<List<Churrasqueira>> GetDisponiveis(DateTime data, int turno);
    Task<List<ChurrasqueiraPacote>> GetPacotes();
    Task<Churrasqueira> GetChurrasqueiraById(int churrasqueiraId);
    Task<ChurrasqueiraPacote> GetChurrasqueiraPacoteById(int pacoteId);
    Task<bool> CadastrarChurrasqueira(Churrasqueira churras);
    Task<bool> UpdateChurrasqueira(Churrasqueira churras);
    Task<bool> CadastrarPacote(ChurrasqueiraPacote churrasPacote);
    Task<bool> UpdatePacote(ChurrasqueiraPacote churrasPacote);
    Task<bool> DeleteChurrasqueira(Churrasqueira churras);
    Task<bool> DeleteChurrasqueiraPacote(ChurrasqueiraPacote itemPacote);
}