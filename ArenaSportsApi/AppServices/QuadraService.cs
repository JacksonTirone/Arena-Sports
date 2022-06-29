using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArenaSportsApi.Models;
using AutoMapper;

public class QuadraService : IQuadraService
{
    private readonly IMapper _mapper;
    private readonly IQuadraRepository _quadraRepo;

    public QuadraService(IMapper mapper, DataContext context, IQuadraRepository quadraRepo)
    {
        _mapper = mapper;
        _quadraRepo = quadraRepo;
    }

    public async Task<ServiceResponse<List<QuadraVMResponse>>> GetAll()
    {
        var quadras = await _quadraRepo.GetAll();
        var quadrasResponse = _mapper.Map<List<QuadraVMResponse>>(quadras);

        return new ServiceResponse<List<QuadraVMResponse>>() { Data = quadrasResponse, Success = true };
    }

    public async Task<ServiceResponse<List<QuadraItemOpcional>>> GetAllOpcionais()
    {
        var opcionais = await _quadraRepo.GetOpcionais();
        return new ServiceResponse<List<QuadraItemOpcional>>() { Success = true, Data = opcionais };
    }

    public async Task<EmptyResponse> CadastrarAtualizarQuadra(UsuarioLogado userLogado, CadastrarQuadraVMRequest requestQuadra)
    {
        var quadra = _mapper.Map<Quadra>(requestQuadra);

        var tupleValidacao = ValidarCadastroQuadra(userLogado, quadra);
        if(!tupleValidacao.Item1)
            return new EmptyResponse() { Message = tupleValidacao.Item2 };
        quadra.Ativo = true;
        var success = false;
        if (quadra.QuadraId > 0)
            success = await _quadraRepo.UpdateQuadra(quadra);
        else
            success = await _quadraRepo.CadastrarQuadra(quadra);
        return new EmptyResponse() { Success = success };
    }

    public async Task<EmptyResponse> CadastrarAtualizarItemQuadra(UsuarioLogado userLogado, QuadraItemOpcionalVMRequest requestItem)
    {
        var itemQuadra = _mapper.Map<QuadraItemOpcional>(requestItem);

        var tupleValidacao = ValidarCadastroItem(userLogado, itemQuadra);
        if(!tupleValidacao.Item1)
            return new EmptyResponse() { Message = tupleValidacao.Item2 };
        itemQuadra.Ativo = true;
        var success = false;
        if (itemQuadra.QuadraItemOpcionalId > 0)
            success = await _quadraRepo.UpdateItemQuadra(itemQuadra);
        else
            success = await _quadraRepo.CadastrarItemQuadra(itemQuadra);

        return new EmptyResponse() { Success = success };
    }

    public async Task<EmptyResponse> DeleteQuadra(UsuarioLogado userLogado, int quadraId)
    {
        var quadra = await _quadraRepo.GetQuadraById(quadraId);
        if(quadra == null)
            return new EmptyResponse() { Message = "Quadra não encontrada." };
        
        quadra.Ativo = false;
        quadra.DataExclusao = DateTime.Now;
        quadra.UsuarioExclusaoId = userLogado.UsuarioId;
        
        return new EmptyResponse() { Success = await _quadraRepo.DeleteQuadra(quadra) };
    }

    public async Task<EmptyResponse> DeleteOpcionalQuadra(UsuarioLogado userLogado, int quadraOpcionalId)
    {
        var opcionalQuadra = await _quadraRepo.GetOpcionalQuadra(quadraOpcionalId);
        if(opcionalQuadra == null)
            return new EmptyResponse() { Message = "Item opcional não encontrado." };
        
        opcionalQuadra.Ativo = false;
        opcionalQuadra.DataExclusao = DateTime.Now;
        opcionalQuadra.UsuarioExclusaoId = userLogado.UsuarioId;
        
        return new EmptyResponse() { Success = await _quadraRepo.DeleteOpcionalQuadra(opcionalQuadra) };
    }

    private Tuple<bool, string> ValidarCadastroQuadra(UsuarioLogado userLogado, Quadra quadra)
    {
        if (userLogado.TipoUsuarioId != (int)eTipoUsuario.Administrador)
            return new Tuple<bool, string>(false, "Usuário sem permissão de acesso.");

        if (string.IsNullOrEmpty(quadra.Descricao))
            return new Tuple<bool, string>(false, "Descrição da quadra é obrigatório.");

        var esportes = Enum.GetValues(typeof(eEsporte)).Cast<eEsporte>().Select(c => (int)c).ToList();
        if (!esportes.Contains(quadra.EsporteId))
            return new Tuple<bool, string>(false, "Esporte não cadastrado no sistema.");

        var pisos = Enum.GetValues(typeof(ePiso)).Cast<ePiso>().Select(c => (int)c).ToList();
        if (!pisos.Contains(quadra.PisoId))
            return new Tuple<bool, string>(false, "Piso não cadastrado no sistema.");

        return new Tuple<bool, string>(true, string.Empty);
    }

    private Tuple<bool, string> ValidarCadastroItem(UsuarioLogado userLogado, QuadraItemOpcional item)
    {
        if (userLogado.TipoUsuarioId != (int)eTipoUsuario.Administrador)
            return new Tuple<bool, string>(false, "Usuário sem permissão de acesso.");

        if (string.IsNullOrEmpty(item.Descricao))
            return new Tuple<bool, string>(false, "Descrição do item é obrigatória.");

        if (item.Valor <= 0)
            return new Tuple<bool, string>(false, "É obrigatório informar um valor para o item.");

        return new Tuple<bool, string>(true, string.Empty);
    }
}