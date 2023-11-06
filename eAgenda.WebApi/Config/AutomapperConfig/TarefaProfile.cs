using eAgenda.Dominio.ModuloTarefa;
using eAgenda.WebApi.ViewModels.ModuloTarefa;

namespace eAgenda.WebApi.Config.AutomapperConfig
{
    public class TarefaProfile : Profile
    {

        public TarefaProfile()
        {
            CreateMap<Tarefa, ListarTarefaViewModel>()
                .ForMember(destino => destino.DataCriacao, opt => opt.MapFrom(origem => origem.DataCriacao.ToShortDateString()));

            CreateMap<Tarefa, VisualizarTarefaViewModel>()
                .ForMember(destino => destino.DataCriacao, opt => opt.MapFrom(origem => origem.DataCriacao.ToShortDateString()));

        }
    }
}
