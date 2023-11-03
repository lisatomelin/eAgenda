using AutoMapper;
using eAgenda.Aplicacao.ModuloContato;
using eAgenda.Dominio;
using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Dominio.ModuloContato;
using eAgenda.Infra.Orm;
using eAgenda.Infra.Orm.ModuloContato;
using eAgenda.WebApi.ViewModels.ModuloDespesa;
using eAgenda.WebApi.ViewModels.ModuloContato;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.WebApi.Controllers
{
    [ApiController]
    [Route("api/contatos")]
    
    public class ContatoController : ControllerBase
    {
        private ServicoContato servicoContato;
        private IMapper mapeador;

        public ContatoController() {


            IConfiguration configuracao = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

            var connectionString = configuracao.GetConnectionString("SqlServer");

            var builder = new DbContextOptionsBuilder<eAgendaDbContext>();

            builder.UseSqlServer(connectionString);

            //builder.UseSqlServer(@"Data Source=(LOCALDB)\MSSQLLOCALDB;Initial Catalog=eAgendaOrm;Integrated Security=True"); Eviar essa string para appsettings.json

            // Refatorado: var contextoPersistencia = new eAgendaDbContext(builder.Options);

            IContextoPersistencia contextoPersistencia = new eAgendaDbContext(builder.Options);

            // Refatorado: var repositorioContato = new RepositorioContatoOrm(contextoPersistencia);

            IRepositorioContato repositorioContato = new RepositorioContatoOrm(contextoPersistencia);

            // Refatorado: var servicoContato = new ServicoContato(repositorioContato, contextoPersistencia);

            servicoContato = new ServicoContato(repositorioContato, contextoPersistencia);

            var automapperconfig = new MapperConfiguration(opt =>
            {
                opt.CreateMap<Contato, ListarContatoViewModel>();
                opt.CreateMap<Contato, VisualizarContatoViewModel>();

                opt.CreateMap<Compromisso, ListarCompromissoViewModel>()
                .ForMember(destino => destino.Data, opt => opt.MapFrom(origem => origem.Data.ToShortDateString()))
                .ForMember(destino => destino.HoraInicio, opt => opt.MapFrom(origem => origem.HoraInicio.ToString(@"hh\:mm")))
                .ForMember(destino => destino.HoraTermino, opt => opt.MapFrom(origem => origem.HoraTermino.ToString(@"hh\:mm")));

                opt.CreateMap<InserirContatoViewModel, Contato>();
                opt.CreateMap<EditarContatoViewModel, Contato>();
            });

            mapeador = automapperconfig.CreateMapper();
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
