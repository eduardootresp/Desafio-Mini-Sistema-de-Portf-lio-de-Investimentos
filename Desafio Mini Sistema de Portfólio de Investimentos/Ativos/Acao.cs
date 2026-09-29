using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio_Mini_Sistema_de_Portfólio_de_Investimentos.Ativos
{
    // I – Interface Segregation Principle - Princípios SOLID 
    public class Acao : AtivoFinanceiro, IGeradorDeRenda, IAtivoNegociavel
    {
        public override string Nome { get; set; } = string.Empty;
        [Required(ErrorMessage = "O campo Valor é obrigatório.")]
        [Range(1, 1000000000000, ErrorMessage = "O valor deve estar entre 1 a 1.000.000.000.000,00.")]
        public int Quantidade { get; set; }
        [Required(ErrorMessage = "O campo Valor é obrigatório.")]
        [Range(1, 1000000000000, ErrorMessage = "O valor deve estar entre 1 a 1.000.000.000.000,00.")]
        public decimal PrecoMedioCompra {  get; set; }
        [Required(ErrorMessage = "O campo Valor é obrigatório.")]
        [Range(1, 1000000000000, ErrorMessage = "O valor deve estar entre 1 a 1.000.000.000.000,00.")]
        public decimal PrecoMercado {  get; set; }
        [Required(ErrorMessage = "O campo Valor é obrigatório.")]
        [Range(1, 1000000000000, ErrorMessage = "O valor deve estar entre 1 a 1.000.000.000.000,00.")]
        public decimal DividendosRecebidos { get; set; }
        [Required(ErrorMessage = "O campo Valor é obrigatório.")]
        [Range(1, 100, ErrorMessage = "O valor deve estar entre 1 e 100.")]
        public decimal DividendosPorAcao { get; set; }
        [Required(ErrorMessage = "O campo Periodicidade é obrigatório.")]
        [RegularExpression("^(Mensal|Trimestral|Anual)$", 
            ErrorMessage = "A periodicidade deve ser 'Mensal', 'Trimestral' ou 'Anual'.")]
        public string Periodicidade { get; set; } // Ex.: "Mensal", "Trimestral", "Anual"
        [Required(ErrorMessage = "O campo Periodicidade é obrigatório.")]
        [Range(1, 100, ErrorMessage = "O valor deve estar entre 1 e 100.")]
        public decimal VariacaoDiaria { get; set; }

        public Acao()
        {
            Nome = string.Empty; // Inicializa Nome para evitar CS8618
            Periodicidade = "Anual";
        }

        public override decimal ValorAtual => Quantidade * PrecoMercado;

        public override decimal ValorInvestido { get => Quantidade * PrecoMedioCompra; set { } }

        public override decimal CalcularRentabilidade()
        {
            if (ValorInvestido == 0m) return 0m;
            decimal ganhoOuPerda = ValorAtual - ValorInvestido;
            // Retorna a porcentagem em valor inteiro
            return (ganhoOuPerda / ValorInvestido);
        }

        public decimal CalcularRendaPeriodica()
        {
            return Quantidade * DividendosPorAcao;
        }

}
}
