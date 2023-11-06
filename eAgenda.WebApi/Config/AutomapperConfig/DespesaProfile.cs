using eAgenda.Dominio.ModuloDespesa;
using eAgenda.WebApi.ViewModels.ModuloDespesa;

namespace eAgenda.WebApi.Config.AutomapperConfig
{
    public class DespesaProfile : Profile
    {

        public DespesaProfile()
        {  
            CreateMap<Despesa, ListarDespesaViewModel>()
                .ForMember(destino => destino.Data, opt => opt.MapFrom(origem => origem.Data.ToShortDateString()));

            CreateMap<Despesa, VisualizarDespesaViewModel>()
                .ForMember(destino => destino.Data, opt => opt.MapFrom(origem => origem.Data.ToShortDateString()));

            CreateMap<InserirDespesaViewModel, Despesa>();

            CreateMap<EditarDespesaViewModel, Despesa>();

            CreateMap<Categoria, ListarCategoriaViewModel>();
        }    
            
        


    }
       
}
