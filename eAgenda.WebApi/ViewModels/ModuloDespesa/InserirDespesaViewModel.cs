using eAgenda.Dominio.ModuloDespesa;

namespace eAgenda.WebApi.ViewModels.ModuloDespesa
{
    public class InserirDespesaViewModel
    {
        public string Descricao { get; set; }

        public decimal Valor { get; set; }

        public DateTime Data { get; set; }

        public FormaPgtoDespesaEnum FormaPagamento { get; set; }
    }
}