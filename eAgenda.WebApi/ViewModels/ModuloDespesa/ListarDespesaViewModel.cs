﻿namespace eAgenda.WebApi.ViewModels.ModuloDespesa
{
    public class ListarDespesaViewModel
    {

        public Guid Id { get; set; }

        public string Descricao { get; set; }

        public decimal Valor { get; set; }

        public DateTime Data { get; set; }
    }
}