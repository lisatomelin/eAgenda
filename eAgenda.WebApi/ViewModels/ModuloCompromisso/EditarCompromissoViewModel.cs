using eAgenda.Dominio.ModuloContato;

namespace eAgenda.WebApi.ViewModels.ModuloDespesa
{
    public class EditarCompromissoViewModel
    {
        public string Assunto { get; set; }
        public string Link { get; set; }
        public DateTime Data { get; set; }
        public string Local { get; set; }
        public string HoraInicio { get; set; }
        public string HoraTermino { get; set; }

        public Contato Contato;
    }
}
