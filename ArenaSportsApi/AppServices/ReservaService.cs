using System;
using System.Linq;
using System.Threading.Tasks;
using ArenaSportsApi.Models;
using AutoMapper;

public class ReservaService : IReservaService
{
    private readonly IMapper _mapper;
    private readonly IQuadraRepository _quadraRepo;
    private readonly IChurrasqueiraRepository _churrasRepo;
    private readonly IReservaRepository _reservaRepo;
    private readonly IComumService _comumService;


    public ReservaService(IMapper mapper, DataContext context, IQuadraRepository quadraRepo,
        IChurrasqueiraRepository churrasRepo, IReservaRepository reservaRepo, IComumService comumService)
    {
        _mapper = mapper;
        _quadraRepo = quadraRepo;
        _churrasRepo = churrasRepo;
        _reservaRepo = reservaRepo;
        _comumService = comumService;
    }

    public async Task<EmptyResponse> RealizarReserva(UsuarioLogado usuarioLogado, RealizarReservaVMRequest request)
    {
        var quadra = await _quadraRepo.GetQuadraById(request.QuadraId);
        if (quadra == null)
            return new EmptyResponse() { Message = "Quadra não encontrada, verifique a quadra selecionada." };
        
        var horarioReserva = quadra.ConfigsHorarioQuadra.Where(x => x.QuadraConfiguracaoHorarioId == request.QuadraConfiguracaoHorarioId).FirstOrDefault();
        if (horarioReserva.TimeInicio > horarioReserva.TimeFim)
        {
            horarioReserva.TimeFim = horarioReserva.TimeFim.Add(new TimeSpan(1, 0, 0, 0));
            if (request.TimeInicio < horarioReserva.TimeInicio)
            {
                request.TimeInicio = request.TimeInicio.Add(new TimeSpan(1, 0, 0, 0));
                request.DataReserva = request.DataReserva.AddDays(1);
            }
        }

        var dataAgora = DateTime.Now;
        if (request.DataReserva.Date < dataAgora.Date)
            return new EmptyResponse() { Message = "Data da reserva não pode ser anterior a hoje." };
        else if (request.DataReserva.Date == dataAgora.Date && request.TimeInicio <= dataAgora.TimeOfDay)
            return new EmptyResponse() { Message = "Data e hora da reserva não pode ser anterior a agora." };

        var reserva = new Reserva()
        {
            DataReserva = request.DataReserva.Date,
            DataRequisicao = DateTime.Now,
            ReservaUsuarioId = usuarioLogado.TipoUsuarioId == (int)eTipoUsuario.Administrador ? request.UsuarioId.Value : usuarioLogado.UsuarioId,
            ValorTotal = horarioReserva.Valor,
            RequisicaoUsuarioId = usuarioLogado.UsuarioId,
            StatusId = (int)eStatus.Reservada
        };

        reserva.Quadras.Add(new ReservaQuadra() {
            QuadraId = quadra.QuadraId,
            TimeInicio = request.TimeInicio,
            TimeFim = request.TimeInicio.Add(horarioReserva.Duracao),
            Valor = horarioReserva.Valor
        });

        foreach (var item in reserva.Quadras)
        {
            if (item.TimeInicio.Days >= 1)
                item.TimeInicio = item.TimeInicio.Add(new TimeSpan(-1, 0, 0, 0));
            if (item.TimeFim.Days >= 1)
                item.TimeFim = item.TimeFim.Add(new TimeSpan(-1, 0, 0, 0));
        }

        if (request.QuadraItemOpcionalIds.Count > 0)
        {
            var opcionais = await _quadraRepo.GetItensOpcionais(request.QuadraItemOpcionalIds);
            foreach (var item in request.QuadraItemOpcionalIds)
            {
                var opicional = opcionais.Where(x => x.QuadraItemOpcionalId == item).FirstOrDefault();

                if (opicional == null)
                    return new EmptyResponse() { Message = "Item opcional da quadra não econtrado." };
                if (reserva.Quadras.FirstOrDefault().Opcionais.Any(x => x.QuadraItemOpcionalId == item))
                    return new EmptyResponse() { Message = "Item opcional duplicado, verifique a lista enviada." };

                reserva.Quadras.FirstOrDefault().Opcionais.Add(new ReservaQuadraOpcional() {
                    QuadraItemOpcionalId = item,
                    Valor = opicional.Valor
                });
                reserva.ValorTotal = reserva.ValorTotal + opicional.Valor;
            }
        }

        if (request.ChurrasqueiraId.HasValue && request.ChurrasqueiraId != 0)
        {
            var churrasqueira = await _churrasRepo.GetChurrasqueiraById(request.ChurrasqueiraId.Value);
            if (churrasqueira == null)
                return new EmptyResponse() { Message = "Churrasqueira não encontrada, verifique a churrasqueira selecionada." };

            if (!request.ChurrasqueiraPacoteId.HasValue)
                return new EmptyResponse() { Message = "Informe o pacote da churrasqueira para seguir com a reserva." };

            var pacoteChurras = await _churrasRepo.GetChurrasqueiraPacoteById(request.ChurrasqueiraPacoteId.Value);
            if (pacoteChurras == null)
                return new EmptyResponse() { Message = "Pacote da churrasqueira não encontrada, verifique o pacote selecionado." };

            reserva.Churrasqueiras.Add(new ReservaChurrasqueira() {
                ChurrasqueiraId = churrasqueira.ChurrasqueiraId,
                ChurrasqueiraPacoteId = pacoteChurras.ChurrasqueiraPacoteId,
                Turno = request.TimeInicio < new TimeSpan(15, 0, 0) ? 1 : 2,
                Valor = pacoteChurras.Valor
            });
            reserva.ValorTotal = reserva.ValorTotal + pacoteChurras.Valor;
            
            if (!_reservaRepo.VerificaHorarioLivreChurrasqueira(churrasqueira.ChurrasqueiraId, request.DataReserva, request.TimeInicio < new TimeSpan(15, 0, 0) ? 1 : 2))
                return new EmptyResponse() { Message = "Desculpe mas a churrasqueira já foi reservada, selecione outra churrasqueira." };
        }

        if (!_reservaRepo.VerificaHorarioLivreQuadra(quadra.QuadraId, request.DataReserva, reserva.Quadras.FirstOrDefault().TimeInicio))
            return new EmptyResponse() { Message = "Desculpe mas o horário da quadra já foi reservado." };

        return new EmptyResponse() { Success = await _reservaRepo.RealizarReserva(reserva) > 0 };
    }

    public async Task<EmptyResponse> CancelarReserva(UsuarioLogado userLogado, int reservaId)
    {
        var reserva = await _reservaRepo.GetReserva(reservaId);
        if (reserva == null)
            return new EmptyResponse() { Message = "Reserva não encontrada." };
        if (reserva.StatusId != (int)eStatus.Reservada)
            return new EmptyResponse() { Message = "Status da reserva não disponível para cancelamento." };
        
        return new EmptyResponse() { Success = await _reservaRepo.CancelarReserva(userLogado, reserva) };
    }
}