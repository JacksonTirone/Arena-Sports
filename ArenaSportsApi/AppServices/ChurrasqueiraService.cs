using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArenaSportsApi.Models;
using AutoMapper;

public class ChurrasqueiraService : IChurrasqueiraService
{
    private readonly IMapper _mapper;
    private readonly IChurrasqueiraRepository _churrasRepo;

    public ChurrasqueiraService(IMapper mapper, DataContext context, IChurrasqueiraRepository churrasRepo)
    {
        _mapper = mapper;
        _churrasRepo = churrasRepo;
    }

    public async Task<ServiceResponse<List<Churrasqueira>>> GetAll()
    {
        var churrasqueiras = await _churrasRepo.Get();
        return new ServiceResponse<List<Churrasqueira>>() { Success = true, Data = churrasqueiras };
    }

    public async Task<ServiceResponse<List<Churrasqueira>>> GetAllDisponiveis(ChurrasqueirasDisponiveisVMRequest request)
    {
        var churrasqueiras = await _churrasRepo.GetDisponiveis(request.DataReserva, request.TimeInicio < new TimeSpan(15, 0, 0) ? 1 : 2);
        return new ServiceResponse<List<Churrasqueira>>() { Success = true, Data = churrasqueiras };
    }

    public async Task<EmptyResponse> CadastrarAtualizarChurrasqueira(UsuarioLogado userLogado, ChurrasqueiraVMRequest requestChurras)
    {
        var churras = _mapper.Map<Churrasqueira>(requestChurras);

        var tupleValidacao = ValidarCadastroChurras(userLogado, churras);
        if(!tupleValidacao.Item1)
            return new EmptyResponse() { Message = tupleValidacao.Item2 };

        churras.Ativo = true;
        var success = false;
        if (churras.ChurrasqueiraId > 0)
            success = await _churrasRepo.UpdateChurrasqueira(churras);
        else
            success = await _churrasRepo.CadastrarChurrasqueira(churras);
        return new EmptyResponse() { Success =  success };
    }

    public async Task<ServiceResponse<List<ChurrasqueiraPacote>>> GetAllPacote()
    {
        var churrasqueiraPacotes = await _churrasRepo.GetPacotes();
        return new ServiceResponse<List<ChurrasqueiraPacote>>() { Success = true, Data = churrasqueiraPacotes };
    }

    public async Task<EmptyResponse> CadastrarAtualizarPacote(UsuarioLogado userLogado, ChurrasqueiraPacoteVMRequest requestChurrasPacote)
    {
        var pacote = _mapper.Map<ChurrasqueiraPacote>(requestChurrasPacote);

        var tupleValidacao = ValidarCadastroChurrasPacote(userLogado, pacote);
        if(!tupleValidacao.Item1)
            return new EmptyResponse() { Message = tupleValidacao.Item2 };

        var success = false;
        if (pacote.ChurrasqueiraPacoteId > 0 )
            success = await _churrasRepo.UpdatePacote(pacote);
        else
            success = await _churrasRepo.CadastrarPacote(pacote);

        return new EmptyResponse() { Success = success };
    }

    public async Task<EmptyResponse> DeletePacoteChurrasqueira(UsuarioLogado userLogado, int id)
    {
        var itemPacote = await _churrasRepo.GetChurrasqueiraPacoteById(id);
        if(itemPacote == null)
            return new EmptyResponse() { Message = "Item Pacote não encontrado" };
        
        itemPacote.Ativo = false;
        itemPacote.DataExclusao = DateTime.Now;
        itemPacote.UsuarioExclusaoId = userLogado.UsuarioId;
        
        return new EmptyResponse() { Success = await _churrasRepo.DeleteChurrasqueiraPacote(itemPacote) };
    }

    public async Task<EmptyResponse> DeleteChurrasqueira(UsuarioLogado userLogado, int id)
    {
        var churras = await _churrasRepo.GetChurrasqueiraById(id);
        if(churras == null)
            return new EmptyResponse() { Message = "Churrasqueira não encontrada" };

        churras.Ativo = false;
        churras.DataExclusao = DateTime.Now;
        churras.UsuarioExclusaoId = userLogado.UsuarioId;

        return new EmptyResponse() { Success = await _churrasRepo.DeleteChurrasqueira(churras) };
    }

    private Tuple<bool, string> ValidarCadastroChurras(UsuarioLogado userLogado, Churrasqueira churras)
    {
        if (userLogado.TipoUsuarioId != (int)eTipoUsuario.Administrador)
            return new Tuple<bool, string>(false, "Usuário sem permissão de acesso.");

        if (string.IsNullOrEmpty(churras.Descricao))
            return new Tuple<bool, string>(false, "Descrição da churrasqueira é obrigatória.");

        return new Tuple<bool, string>(true, string.Empty);
    }

    private Tuple<bool, string> ValidarCadastroChurrasPacote(UsuarioLogado userLogado, ChurrasqueiraPacote pacote)
    {
        if (userLogado.TipoUsuarioId != (int)eTipoUsuario.Administrador)
            return new Tuple<bool, string>(false, "Usuário sem permissão de acesso.");

        if (string.IsNullOrEmpty(pacote.Descricao))
            return new Tuple<bool, string>(false, "Descrição da churrasqueira é obrigatória.");
        
        if (pacote.Valor <= 0)
            return new Tuple<bool, string>(false, "É obrigatório informar um valor para o pacote.");

        return new Tuple<bool, string>(true, string.Empty);
    }
}