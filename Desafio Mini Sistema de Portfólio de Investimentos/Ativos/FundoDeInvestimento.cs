using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio_Mini_Sistema_de_Portfólio_de_Investimentos.Ativos
{
    internal class FundoDeInvestimento : AtivoFinanceiro, IGeradorDeRenda
    {
        public override string Nome { get; set; } = string.Empty;
        [Required(ErrorMessage = "O campo Valor é obrigatório.")]
        [Range(1, 1000000000000, ErrorMessage = "O valor deve estar entre 1 a 1.000.000.000.000,00.")]
        public decimal QuantidadeCotas { get; set; }
        [Required(ErrorMessage = "O campo Valor é obrigatório.")]
        [Range(1, 1000000000000, ErrorMessage = "O valor deve estar entre 1 a 1.000.000.000.000,00.")]
        public decimal ValorCotaCompra { get; set; }
        [Required(ErrorMessage = "O campo Valor é obrigatório.")]
        [Range(1, 1000000000000, ErrorMessage = "O valor deve estar entre 1 a 1.000.000.000.000,00.")]
        public decimal ValorCotaAtual { get; set; }
        [Required(ErrorMessage = "O campo Valor é obrigatório.")]
        [Range(1, 100, ErrorMessage = "O valor deve estar entre 1 e 100.")]
        public decimal TaxaAdministracao { get; set; }
        [Required(ErrorMessage = "O campo Valor é obrigatório.")]
        [Range(1, 100, ErrorMessage = "O valor deve estar entre 1 e 100.")]
        public decimal RendimentoPorCota { get; set; }
        [RegularExpression("^(Mensal|Trimestral|Anual)$",
        ErrorMessage = "A periodicidade deve ser 'Mensal', 'Trimestral' ou 'Anual'.")]
        public string Periodicidade { get; set; } = "Mensal";

        public override decimal ValorInvestido { get => QuantidadeCotas * ValorCotaCompra; set { } }
        public override decimal ValorAtual => QuantidadeCotas * ValorCotaAtual;
        public override decimal CalcularRentabilidade()
        {
            return ((ValorCotaAtual - ValorCotaCompra) / ValorCotaCompra);
        }
        public decimal CalcularRendaPeriodica()
        {
            return QuantidadeCotas * RendimentoPorCota;
        }

    }
}
