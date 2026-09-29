using Nest;

namespace Desafio_Mini_Sistema_de_Portfólio_de_Investimentos.Services
{
    public class Rentabilidade
    {
        private readonly IRepository<IAtivoFinanceiro> portfolio;
        public Rentabilidade(IRepository<IAtivoFinanceiro> portfolio)
        {
            this.portfolio = portfolio ?? throw new ArgumentNullException(nameof(portfolio));
        }


        public decimal ValorTotalDoPortolio()
        {
            IEnumerable<IAtivoFinanceiro> todosAtivos = portfolio.GetAll();

            decimal valorTotal = 0;
            foreach (var a in todosAtivos)
            {
                string aNome = a.Nome;
                //Console.WriteLine($"{valorTotal} + {a.ValorAtual}");
                valorTotal += a.ValorAtual;
                //Console.WriteLine($"Nome: {aNome} | Valor Atual: {valorTotal.ToString("C2")}");
            }
            return valorTotal;
        }

        public decimal PesoDoAtivo(decimal valorAtual)
        {
            var valorAtualDoAtivo = valorAtual;
            decimal total = ValorTotalDoPortolio();
            decimal pesoDoAtivo = 0;
            pesoDoAtivo = valorAtualDoAtivo / total;
            return pesoDoAtivo;
        }


        public decimal CalcularRentabilidadeMediaPonderada()
        {
            IEnumerable<IAtivoFinanceiro> todosAtivos = portfolio.GetAll();
            var valorTotal = todosAtivos.Sum(a => a.ValorAtual);

            if (valorTotal == 0)
                return 0;

            decimal valorTotalPonderado = todosAtivos.Sum(
                a => a.CalcularRentabilidade() * a.ValorAtual
            ) / valorTotal;
            return valorTotalPonderado;
        }

    }
        
}

