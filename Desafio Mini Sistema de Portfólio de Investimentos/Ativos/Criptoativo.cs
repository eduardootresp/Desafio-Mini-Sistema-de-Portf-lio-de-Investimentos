using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio_Mini_Sistema_de_Portfólio_de_Investimentos.Ativos
{
    internal class Criptoativo : AtivoFinanceiro, IAtivoNegociavel
    {
        public override string Nome { get; set; } = string.Empty;
        [Required(ErrorMessage = "O campo Valor é obrigatório.")]
        [Range(1, 1000000000000, ErrorMessage = "O valor deve estar entre 1 a 1.000.000.000.000,00.")]
        public decimal Quantidade { get; set; }
        [Required(ErrorMessage = "O campo Valor é obrigatório.")]
        [Range(1, 1000000000000, ErrorMessage = "O valor deve estar entre 1 a 1.000.000.000.000,00.")]
        public decimal PrecoMedioCompra {  get; set; }
        [Required(ErrorMessage = "O campo Valor é obrigatório.")]
        [Range(1, 1000000000000, ErrorMessage = "O valor deve estar entre 1 a 1.000.000.000.000,00.")]
        public decimal PrecoMercado { get; set; }
        public override decimal ValorInvestido { get => Quantidade * PrecoMedioCompra; set { } }
        public override decimal ValorAtual => Quantidade * PrecoMercado;
        [Required(ErrorMessage = "O campo Periodicidade é obrigatório.")]
        [Range(1, 100, ErrorMessage = "O valor deve estar entre 1 e 100.")]
        public decimal VariacaoDiaria { get; set; }

        public override decimal CalcularRentabilidade()
        {
            return ((ValorAtual - ValorInvestido)
            / ValorInvestido);
        }
    }
}
