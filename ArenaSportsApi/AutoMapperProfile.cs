using ArenaSportsApi.Models;
using AutoMapper;

namespace ArenaSportsApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CadastrarQuadraVMRequest, Quadra>();
            CreateMap<ConfigHorarioQuadraVMRequest, QuadraConfiguracaoHorario>();
            CreateMap<ChurrasqueiraVMRequest, Churrasqueira>();
            CreateMap<ChurrasqueiraPacoteVMRequest, ChurrasqueiraPacote>();
            CreateMap<QuadraItemOpcionalVMRequest, QuadraItemOpcional>();
            CreateMap<Quadra, QuadraVMResponse>();
            CreateMap<Usuario, ClienteVMResponse>();
        }
    }
}