using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.WebApi.ViewModels.ModuloDespesa;

namespace eAgenda.WebApi.ViewModels.ModuloDespesa
{
    public class VisualizarDespesaViewModel
    {    

        public VisualizarDespesaViewModel()
        {
            categorias = new List<ListarCategoriaViewModel>();
           
        }

        public Guid Id { get; set; }
        public string Descricao { get; set; }

        public decimal Valor { get; set; }

        public DateTime Data { get; set; }

        public FormaPgtoDespesaEnum FormaPagamento { get; set; }       

        public List<ListarCategoriaViewModel> categorias { get; set; }
    }
}