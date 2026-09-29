using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Desafio_Mini_Sistema_de_Portfólio_de_Investimentos.Ativos
{
    public abstract class AtivoFinanceiro : IAtivoFinanceiro
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ0-9\s]+$",
        ErrorMessage = "Use apenas letras e números.")]
        public abstract string Nome { get; set; }
        [Required(ErrorMessage = "O campo Valor é obrigatório.")]
        [Range(1, 1000000000000, ErrorMessage = "O valor deve estar entre 1 a 1.000.000.000.000,00.")]

        public abstract decimal ValorInvestido { get; set; }

        public abstract decimal ValorAtual { get; }

        public abstract decimal CalcularRentabilidade();

        public override string ToString()
        {
            var props = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var sb = new StringBuilder();
            foreach ( var p in props )
            {
                object val;
                try
                {
                    val = p.GetValue(this) ?? "null";
                }
                catch
                {
                    val = "<erro ao obter>";
                }
                sb.Append($" {p.Name}: {val}\n");
            }
            sb.Append("\n======================================\n");
            return sb.ToString().TrimEnd();
        }
    }
}
