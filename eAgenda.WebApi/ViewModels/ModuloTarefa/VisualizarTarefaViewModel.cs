using eAgenda.Dominio.ModuloTarefa;

namespace eAgenda.WebApi.ViewModels.ModuloTarefa
{
    public class VisualizarTarefaViewModel
    {
        public List<ItemTarefa> itens;

        public VisualizarTarefaViewModel()
        {
            itens = new List<ItemTarefa>();

        }
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public PrioridadeTarefaEnum Prioridade { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataConclusao { get; set; }        
        public decimal PercentualConcluido { get; set; }

        public List<ItemTarefa> Itens { get { return itens; } }
    }
}