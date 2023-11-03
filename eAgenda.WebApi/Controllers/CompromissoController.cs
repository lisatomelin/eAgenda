using eAgenda.Dominio.ModuloContato;
using eAgenda.Dominio;
using eAgenda.Infra.Orm.ModuloContato;
using eAgenda.Infra.Orm;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eAgenda.Aplicacao.ModuloContato;
using eAgenda.Aplicacao.ModuloCompromisso;
using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.WebApi.ViewModels.ModuloContato;
using eAgenda.WebApi.ViewModels.ModuloDespesa;
using eAgenda.Infra.Orm.ModuloCompromisso;
using Microsoft.Win32;
using AutoMapper;

namespace eAgenda.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class CompromissoController : ControllerBase
    {
        private ServicoCompromisso servicoCompromisso;
        private IMapper mapeador;

        public CompromissoController() 
        {


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

            IRepositorioCompromisso repositorioCompromisso = new RepositorioCompromissoOrm(contextoPersistencia);

            // Refatorado: var servicoContato = new ServicoContato(repositorioContato, contextoPersistencia);

            servicoCompromisso = new ServicoCompromisso(repositorioCompromisso, contextoPersistencia);

            var automapperconfig = new MapperConfiguration(opt =>
            {
                opt.CreateMap<Compromisso, ListarCompromissoViewModel>()
                .ForMember(destino => destino.Data, opt => opt.MapFrom(origem => origem.Data.ToShortDateString()))
                .ForMember(destino => destino.HoraInicio, opt => opt.MapFrom(origem => origem.HoraInicio.ToString(@"hh\:mm")))
                .ForMember(destino => destino.HoraTermino, opt => opt.MapFrom(origem => origem.HoraTermino.ToString(@"hh\:mm")));

                opt.CreateMap<Compromisso, VisualizarCompromissoViewModel>()
                .ForMember(destino => destino.Data, opt => opt.MapFrom(origem => origem.Data.ToShortDateString()))
                .ForMember(destino => destino.HoraInicio, opt => opt.MapFrom(origem => origem.HoraInicio.ToString(@"hh\:mm")))
                .ForMember(destino => destino.HoraTermino, opt => opt.MapFrom(origem => origem.HoraTermino.ToString(@"hh\:mm")));

                opt.CreateMap<InserirCompromissoViewModel, Compromisso>();
                opt.CreateMap<EditarCompromissoViewModel, Compromisso>();
            });

            mapeador = automapperconfig.CreateMapper();
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
