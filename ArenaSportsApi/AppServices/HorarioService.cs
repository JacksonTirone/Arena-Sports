using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArenaSportsApi.Models;
using AutoMapper;

public class HorarioService : IHorarioService
{
    private readonly IMapper _mapper;
    private readonly IQuadraRepository _quadraRepo;
    private readonly IHorarioRepository _horarioRepo;
    private readonly IReservaRepository _reservaRepo;
    private readonly IComumService _comumService;
    private readonly DateTime dateToday;

    public HorarioService(IMapper mapper, DataContext context, IQuadraRepository quadraRepo, IHorarioRepository horarioRepo,
        IReservaRepository reservaRepo, IComumService comumService)
    {
        _mapper = mapper;
        _quadraRepo = quadraRepo;
        _horarioRepo = horarioRepo;
        _reservaRepo = reservaRepo;
        _comumService = comumService;
        dateToday = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
    }

    public async Task<EmptyResponse> ConfigHorarioQuadra(UsuarioLogado userLogado, ConfigHorarioQuadraVMRequest requestHoraConfig)
    {
        var configHoraRequest = _mapper.Map<QuadraConfiguracaoHorario>(requestHoraConfig);

        var tupleValicao = ValidarConfigHorario(userLogado, configHoraRequest);
        if (!tupleValicao.Item1)
            return new EmptyResponse() { Message = tupleValicao.Item2 };

        var quadra = await _quadraRepo.GetQuadraById(configHoraRequest.QuadraId);
        if (quadra == null)
            return new EmptyResponse() { Message= "Quadra não encontrada." };

        if (!_comumService.ValidarIntervaloHorario(quadra, configHoraRequest.DiaSemana, configHoraRequest.TimeInicio, configHoraRequest.TimeFim))
            return new EmptyResponse() { Message= "Intervalo de horários coincide com um horário já adicionado, verifique os horários informados." };

        return new EmptyResponse() { Success = await _horarioRepo.AddConfigQuadra(configHoraRequest) > 0 };
    }

    private Tuple<bool, string> ValidarConfigHorario(UsuarioLogado userLogado, QuadraConfiguracaoHorario configHora)
    {
        if (userLogado.TipoUsuarioId != (int)eTipoUsuario.Administrador)
            return new Tuple<bool, string>(false, "Usuário sem permissão de acesso.");

        var diaSemana = Enum.GetValues(typeof(eDiaSemana)).Cast<eDiaSemana>().Select(c => (int)c).ToList();
        if (!diaSemana.Contains(configHora.DiaSemana))
            return new Tuple<bool, string>(false, "Dia da semana não encontrado, utilize 0 - Dom, 1 - Seg, 2 - Ter, etc...");

        if (configHora.Duracao < TimeSpan.FromMinutes(30) || configHora.Duracao > TimeSpan.FromMinutes(240))
            return new Tuple<bool, string>(false, "Duração inválida, duração deve ter no mínimo 30 e máximo de 2 horas e 30 minutos.");

        if (configHora.TimeInicio == configHora.TimeFim)
            return new Tuple<bool, string>(false, "Horário inicial e final não podem ser iguais.");

        var horaInicial = configHora.TimeInicio;
        for (int i = 0; i < 1440 / configHora.Duracao.TotalMinutes; i++)
        {
            horaInicial = horaInicial.Add(configHora.Duracao);
            if (horaInicial.Hours == configHora.TimeFim.Hours && horaInicial.Minutes == configHora.TimeFim.Minutes)
                return new Tuple<bool, string>(true, string.Empty);
        }

        return new Tuple<bool, string>(false, "Horários inválidos, verifique os períodos informados. Hora inicial e final tem que estar dentro do intervalo de duração. Exemplo: Inicio - 16:00. Final - 22:00. Duração: 60.");
    }

    public async Task<ServiceResponse<List<QuadraHorariosVMResponse>>> GetHorariosQuadras(HorariosQuadrasVMRequest requestQuadraHoraRequest)
    {
        var dayOfWeek = _comumService.GetDayOfWeek(requestQuadraHoraRequest.Data);
        var quadras = await _horarioRepo.GetQuadrasHorariosByDayOfWeek(dayOfWeek);
        var reservasDoDia = await _reservaRepo.GetReservaQuadrasByData(requestQuadraHoraRequest.Data, quadras.Select(x => x.QuadraId).ToList());
        var reservasDoProximoDia = await _reservaRepo.GetReservaQuadrasByData(requestQuadraHoraRequest.Data.AddDays(1), quadras.Select(x => x.QuadraId).ToList());
        return new ServiceResponse<List<QuadraHorariosVMResponse>>() {
            Success = true, 
            Data = QuadraAdapter.ModelToViewModel(quadras, requestQuadraHoraRequest.Data, reservasDoDia, reservasDoProximoDia)
        };
    }

    public async Task<ServiceResponse<List<QuadraConfiguracaoHorario>>> GetConfigHorario(UsuarioLogado userLogado, ConfiguracaoHorarioVMRequest request)
    {
        var quadra = await _quadraRepo.GetQuadraById(request.QuadraId);
        if (quadra == null)
            return new ServiceResponse<List<QuadraConfiguracaoHorario>>() { Message= "Quadra não encontrada." };

        if (request.DiaSemana < 0 || request.DiaSemana > 6)
            return new ServiceResponse<List<QuadraConfiguracaoHorario>>() { Message= "Dia da semana inválido, 0 - Domingo, 1 - Segunda, 6 - Sábado." };

        return new ServiceResponse<List<QuadraConfiguracaoHorario>>() { Success = true, Data = await _quadraRepo.GetConfigHorario(request.QuadraId, request.DiaSemana) };
    }

    public async Task<EmptyResponse> DeleteConfigHorario(UsuarioLogado userLogado, int quadraConfigHoraId)
    {
        var configHora = await _horarioRepo.GetConfigQuadraById(quadraConfigHoraId);
        if(configHora == null)
            return new EmptyResponse() { Message = "Configuração de horário não encontrado." };
        return new EmptyResponse() { Success = await _horarioRepo.DeleteConfigHora(configHora) };
    }
}