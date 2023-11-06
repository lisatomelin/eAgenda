using Microsoft.AspNetCore.Mvc;
using eAgenda.Aplicacao.ModuloTarefa;
using eAgenda.Dominio.ModuloTarefa;
using eAgenda.WebApi.ViewModels.ModuloTarefa;

namespace eAgenda.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefaController : ControllerBase
    {
        public ServicoTarefa servicoTarefa;

        public IMapper mapeador;
        public TarefaController(ServicoTarefa servicoTarefa, IMapper mapeador)
        {
            this.mapeador = mapeador;
            this.servicoTarefa = servicoTarefa;           
            
        }


        [HttpGet]
        public List<ListarTarefaViewModel> SelecionarTodos(StatusTarefaEnum statusSelecionado)
        {

            List<Tarefa> tarefas = servicoTarefa.SelecionarTodos(statusSelecionado).Value;

            List<ListarTarefaViewModel> tarefasViewModel = mapeador.Map<List<ListarTarefaViewModel>>(tarefas);

            return tarefasViewModel;
        }


        [HttpGet("visualizacao-completa/{id}")]
        public VisualizarTarefaViewModel SelecionarPorId(Guid id)
        {
            var tarefa = servicoTarefa.SelecionarPorId(id).Value;

            var tarefaViewModel = mapeador.Map<VisualizarTarefaViewModel>(tarefa);

            return tarefaViewModel;

        }

        [HttpPost]
        public string Inserir(InserirTarefaViewModel tarefaViewModel)
        {
            var tarefa = mapeador.Map<Tarefa>(tarefaViewModel);

            var resultado = servicoTarefa.Inserir(tarefa);

            if (resultado.IsSuccess)
                return "Tarefa inserida com sucesso";

            string[] erros = resultado.Errors.Select(x => x.Message).ToArray();

            return string.Join("\r\n", erros);
        }

        [HttpPut("{id}")]
        public string Editar(Guid id, EditarTarefaViewModel tarefaViewModel)
        {
            var tarefaEncontrada = servicoTarefa.SelecionarPorId(id).Value;

            var tarefa = mapeador.Map(tarefaViewModel, tarefaEncontrada);

            var resultado = servicoTarefa.Editar(tarefa);

            if (resultado.IsSuccess)
                return "Tarefa editada com sucesso";

            string[] erros = resultado.Errors.Select(x => x.Message).ToArray();

            return string.Join("\r\n", erros);
        }

        [HttpDelete("{id}")]
        public string Excluir(Guid id)
        {
            var resultadoBusca = servicoTarefa.SelecionarPorId(id);

            if (resultadoBusca.IsFailed)
            {
                string[] errosNaBusca = resultadoBusca.Errors.Select(x => x.Message).ToArray();

                return string.Join("\r\n", errosNaBusca);
            }

            var tarefa = resultadoBusca.Value;

            var resultado = servicoTarefa.Excluir(tarefa);

            if (resultado.IsSuccess)
                return "Tarefa excluída com sucesso";

            string[] erros = resultado.Errors.Select(x => x.Message).ToArray();

            return string.Join("\r\n", erros);
        }
    }
}
