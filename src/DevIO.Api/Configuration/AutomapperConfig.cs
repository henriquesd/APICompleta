using AutoMapper;
using DevIO.Api.ViewModels;
using DevIO.Business.Models;

namespace DevIO.Api.Configuration
{
    // Profile é uma classe do AutoMapper - está dizendo que é uma classe de perfil; então tudo que tiver
    // aqui dentro, vai ser resolvido pelo AutoMapper na hora que inicializar a aplicação;
    // Essa classe AutomapperConfig não vai ser inicializada em nenhum lugar; O que acontece é que na hora que o Startup
    // fizer o ciclo de configuração, o AutoMapper irá olhar para este assembly e para tudo que fizer parte deste assembly ele irá buscar
    // por interfaces ou implementações que implementam a herança do Profile e irá resolver todos os mapeamentos que estiverem dentro
    // deste assembly - ele faz isso automático;
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            // Tem que criar um outro mapping de FornecedorViewModel para Fornecedor; se as duas formas de criar
            // forem a mesma, tanto faz a ordem, não tem construtor, não tem parâmetros, pode utilizar o ReverseMap;
            // o ReverseMap irá fazer o mapeamento também no sentido inverso;
            // CreateMap<Fornecedor, FornecedorViewModel>();

            CreateMap<Fornecedor, FornecedorViewModel>().ReverseMap();
            CreateMap<Endereco, EnderecoViewModel>().ReverseMap();
            CreateMap<ProdutoViewModel, Produto>();

            CreateMap<ProdutoImagemViewModel, Produto>().ReverseMap();


            CreateMap<Produto, ProdutoViewModel>()
                .ForMember(dest => dest.NomeFornecedor, opt => opt.MapFrom(src => src.Fornecedor.Nome));
        }
    }
}
