using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio_Mini_Sistema_de_Portfólio_de_Investimentos.Ativos
{
    internal class TituloRendaFixa : AtivoFinanceiro, IAtivoComVencimento
    {
        public override string Nome { get ; set ; } = string.Empty; // Inicializa Nome para evitar CS8618

        public decimal _valorInvestido;
        public override decimal ValorInvestido
        {
            get => _valorInvestido;
            set => _valorInvestido = value;
        }
        [Required(ErrorMessage = "O campo Valor é obrigatório.")]
        [Range(1, 100, ErrorMessage = "O valor deve estar entre 1 e 100.")]
        public decimal TaxaAnual { get; set; }
        public override decimal ValorAtual => ValorInvestido * (1 + Rentabilidade / 100);
        
        public decimal Rentabilidade => CalcularRentabilidade();
        [Required(ErrorMessage = "O campo Valor é obrigatório.")]
        public DateTime DataAplicacao { get; set; }
        [Required(ErrorMessage = "O campo Valor é obrigatório.")]
        public DateTime DataVencimento { get; set; }

        public decimal DiasDecorridos()
        {
            DateTime hoje = DateTime.Today;
            return (hoje - DataAplicacao).Days;
        }

        public override decimal CalcularRentabilidade()
        {
            return (TaxaAnual * (DiasDecorridos() / 365)/100);
        }

        public int DiasParaVencimento()
        {
            DateTime hoje = DateTime.Today;
            return ((DataVencimento - hoje)).Days;
        }
    }
}
