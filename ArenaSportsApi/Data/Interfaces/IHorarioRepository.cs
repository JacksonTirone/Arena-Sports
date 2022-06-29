using System.Collections.Generic;
using System.Threading.Tasks;
using ArenaSportsApi.Models;

public interface IHorarioRepository
{
    Task<QuadraConfiguracaoHorario> GetConfigQuadraById(int configHoraQuadraId);
    Task<int> AddConfigQuadra(QuadraConfiguracaoHorario configHoraQuadra);
    Task<List<Quadra>> GetQuadrasHorariosByDayOfWeek(int dayOfWeek);
    Task<bool> DeleteConfigHora(QuadraConfiguracaoHorario configHora);
}