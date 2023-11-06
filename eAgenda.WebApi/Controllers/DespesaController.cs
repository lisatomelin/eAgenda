using Microsoft.AspNetCore.Mvc;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Aplicacao.ModuloDespesa;
using eAgenda.WebApi.ViewModels.ModuloDespesa;

namespace eAgenda.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DespesaController : ControllerBase
    {
        private IMapper mapeador;
        private ServicoDespesa servicoDespesa;

        public DespesaController(ServicoDespesa servicoDespesa, IMapper mapeador)
        {
            this.mapeador = mapeador;
            this.servicoDespesa = servicoDespesa;            
            
        }

        [HttpGet]
        public List<ListarDespesaViewModel> SelecionarTodos()
        {

            List<Despesa> despesas = servicoDespesa.SelecionarTodos().Value;

            List<ListarDespesaViewModel> despesaViewModel = mapeador.Map<List<ListarDespesaViewModel>>(despesas);

            return despesaViewModel;
        }


        [HttpGet("visualizacao-completa/{id}")]
        public VisualizarDespesaViewModel SelecionarPorId(Guid id)
        {
            var despesa = servicoDespesa.SelecionarPorId(id).Value;

            var despesaViewModel = mapeador.Map<VisualizarDespesaViewModel>(despesa);

            return despesaViewModel;

        }

        [HttpPost]
        public string Inserir(InserirDespesaViewModel despesaViewModel)
        {
            var despesa = mapeador.Map<Despesa>(despesaViewModel);

            var resultado = servicoDespesa.Inserir(despesa);

            if (resultado.IsSuccess)
                return "Despesa inserida com sucesso";

            string[] erros = resultado.Errors.Select(x => x.Message).ToArray();

            return string.Join("\r\n", erros);
        }


        [HttpPut("{id}")]
        public string Editar(Guid id, EditarDespesaViewModel despesaViewModel)
        {
            var despesaEncontrada = servicoDespesa.SelecionarPorId(id).Value;

            var despesa = mapeador.Map(despesaViewModel,despesaEncontrada);

            var resultado = servicoDespesa.Editar(despesa);

            if (resultado.IsSuccess)
                return "Despesa editada com sucesso";

            string[] erros = resultado.Errors.Select(x => x.Message).ToArray();

            return string.Join("\r\n", erros);
        }


        [HttpDelete("{id}")]
        public string Excluir(Guid id)
        {
            var resultadoBusca = servicoDespesa.SelecionarPorId(id);

            if (resultadoBusca.IsFailed)
            {
                string[] errosNaBusca = resultadoBusca.Errors.Select(x => x.Message).ToArray();

                return string.Join("\r\n", errosNaBusca);
            }

            var despesa = resultadoBusca.Value;

            var resultado = servicoDespesa.Excluir(despesa);

            if (resultado.IsSuccess)
                return "Despesa excluída com sucesso";

            string[] erros = resultado.Errors.Select(x => x.Message).ToArray();

            return string.Join("\r\n", erros);
        }

    }
}

