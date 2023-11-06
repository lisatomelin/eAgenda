using eAgenda.Aplicacao.ModuloContato;
using eAgenda.Dominio.ModuloContato;
using eAgenda.WebApi.ViewModels.ModuloContato;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApi.Controllers
{
    [ApiController]
    [Route("api/contatos")]
    
    public class ContatoController : ControllerBase
    {
        private ServicoContato servicoContato;
        private IMapper mapeador;

        public ContatoController(ServicoContato servicoContato, IMapper mapeador) {

            this.mapeador = mapeador;
            this.servicoContato = servicoContato;
            
        }          

        [HttpGet]
        public List<ListarContatoViewModel> SelecionarTodos(StatusFavoritoEnum statusFavorito) { 

            List<Contato> contatos = servicoContato.SelecionarTodos(statusFavorito).Value;

            List<ListarContatoViewModel> contatosViewModel = mapeador.Map<List<ListarContatoViewModel>>(contatos);

            return contatosViewModel;
        }


        [HttpGet("visualizacao-completa/{id}")]
        public VisualizarContatoViewModel SelecionarPorId(Guid id)
        {
            var contato =  servicoContato.SelecionarPorId(id).Value;

            var contatoViewModel = mapeador.Map<VisualizarContatoViewModel>(contato);            

            return contatoViewModel;

        }

        [HttpPost]
        public string Inserir(InserirContatoViewModel contatoViewModel)
        {
            var contato = mapeador.Map<Contato>(contatoViewModel);

            var resultado = servicoContato.Inserir(contato);

            if (resultado.IsSuccess)
              return "Contato inserido com sucesso";

            string[] erros = resultado.Errors.Select(x => x.Message).ToArray();

            return string.Join("\r\n", erros);
        }


        [HttpPut("{id}")]
        public string Editar(Guid id, EditarContatoViewModel contatoViewModel)
        {
            var contatoEncontrado = servicoContato.SelecionarPorId(id).Value;

            var contato = mapeador.Map(contatoViewModel, contatoEncontrado);

            var resultado = servicoContato.Editar(contato);

            if (resultado.IsSuccess)
                return "Contato editado com sucesso";

            string[] erros = resultado.Errors.Select(x => x.Message).ToArray();

            return string.Join("\r\n", erros);
        }


        [HttpDelete("{id}")]
        public string Excluir(Guid id)
        {
            var resultadoBusca = servicoContato.SelecionarPorId(id);

            if (resultadoBusca.IsFailed)
            {
                string[] errosNaBusca = resultadoBusca.Errors.Select(x => x.Message).ToArray();

                return string.Join("\r\n", errosNaBusca);
            }

            var contato = resultadoBusca.Value;

            var resultado = servicoContato.Excluir(contato);

            if (resultado.IsSuccess)
                return "Contato excluído com sucesso";

            string[] erros = resultado.Errors.Select(x => x.Message).ToArray();

            return string.Join("\r\n", erros);
        }

    }



}
