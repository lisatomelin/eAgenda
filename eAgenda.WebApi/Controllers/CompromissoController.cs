using Microsoft.AspNetCore.Mvc;
using eAgenda.Aplicacao.ModuloCompromisso;
using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.WebApi.ViewModels.ModuloDespesa;

namespace eAgenda.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class CompromissoController : ControllerBase
    {
        private ServicoCompromisso servicoCompromisso;
        private IMapper mapeador;

        public CompromissoController(ServicoCompromisso servicoCompromisso, IMapper mapeador) 
        {

            this.servicoCompromisso = servicoCompromisso;
            this.mapeador = mapeador;
                       
        }

        [HttpGet]
        public List<ListarCompromissoViewModel> SelecionarTodos()
        {          

            List<Compromisso> compromisso = servicoCompromisso.SelecionarTodos().Value;

            List<ListarCompromissoViewModel> compromissosViewModel = mapeador.Map<List<ListarCompromissoViewModel>>(compromisso);

            return compromissosViewModel;
        }


        [HttpGet("visualizacao-completa/{id}")]
        public VisualizarCompromissoViewModel SelecionarPorId(Guid id)
        {
            var compromisso = servicoCompromisso.SelecionarPorId(id).Value;

            var compromissosViewModel = mapeador.Map<VisualizarCompromissoViewModel>(compromisso);          


            return compromissosViewModel;

        }

        [HttpPost]
        public string Inserir(InserirCompromissoViewModel compromissosViewModel)
        {

            var compromisso = mapeador.Map<Compromisso>(compromissosViewModel);

            var resultado = servicoCompromisso.Inserir(compromisso);

            if (resultado.IsSuccess)
                return "Compromisso foi inserido com sucesso";

            string[] erros = resultado.Errors.Select(x => x.Message).ToArray();

            return string.Join("\r\n", erros);
        }

        [HttpPut("{id}")]
        public string Editar(Guid id, EditarCompromissoViewModel compromissoViewModel)
        {
            var compromissoEncontrado = servicoCompromisso.SelecionarPorId(id).Value;

            var compromisso = mapeador.Map(compromissoViewModel, compromissoEncontrado);

            var resultado = servicoCompromisso.Editar(compromisso);

            if (resultado.IsSuccess)
                return "COmpromisso editado com sucesso";

            string[] erros = resultado.Errors.Select(x => x.Message).ToArray();

            return string.Join("\r\n", erros);
        }



        [HttpDelete("{id}")]
        public string Excluir(Guid id)
        {
            var resultadoBusca = servicoCompromisso.SelecionarPorId(id);

            if (resultadoBusca.IsFailed)
            {
                string[] errosNaBusca = resultadoBusca.Errors.Select(x => x.Message).ToArray();

                return string.Join("\r\n", errosNaBusca);
            }

            var compromisso = resultadoBusca.Value;

            var resultado = servicoCompromisso.Excluir(compromisso);

            if (resultado.IsSuccess)
                return "Compromisso excluído com sucesso";

            string[] erros = resultado.Errors.Select(x => x.Message).ToArray();

            return string.Join("\r\n", erros);
        }
    }
}
