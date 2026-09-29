using Desafio_Mini_Sistema_de_Portfólio_de_Investimentos.Ativos;
using Desafio_Mini_Sistema_de_Portfólio_de_Investimentos.Repository;
using Desafio_Mini_Sistema_de_Portfólio_de_Investimentos.Services;
using Desafio_Mini_Sistema_de_Portfólio_de_Investimentos.GeradorDeRelatorio;
using Elasticsearch.Net;
using Nest;
using System.Drawing;
using System.Linq.Expressions;
using System.Reflection;

namespace Desafio_Mini_Sistema_de_Portfólio_de_Investimentos
{
    // Comentários do Uso do SOLID sempre seguido de " - Princípios SOLID"
    internal class Program
    {
        static void Main(string[] args)
        {
            Type tipoAberto = typeof(Portfolio<>);
            Type tipoFechado = tipoAberto.MakeGenericType(typeof(IAtivoFinanceiro));
            object instanciaObjeto = Activator.CreateInstance(tipoFechado);
            IRepository<IAtivoFinanceiro> portfolio = (IRepository<IAtivoFinanceiro>)instanciaObjeto;

            // 1. inserção de dados 
            portfolio.AddOrUpdate(new Acao
            {
                Nome = "PETROBRAS PN",
                Quantidade = 100,
                PrecoMedioCompra = 30.00m,
                PrecoMercado = 32.50m,
                DividendosRecebidos = 100,
                DividendosPorAcao = 0.80m,
                Periodicidade = "Anual",
                VariacaoDiaria = 1.25m
            });

            portfolio.AddOrUpdate(new FundoDeInvestimento
            {
                Nome = "FIC PLENO",
                QuantidadeCotas = 100,
                ValorCotaCompra = 80.00m,
                ValorCotaAtual = 84.20m,
                TaxaAdministracao = 1.2m,
                RendimentoPorCota = 1.20m,
                Periodicidade = "Mensal"
            });

            portfolio.AddOrUpdate(new Criptoativo
            {
                Nome = "BITCOIN",
                Quantidade = 50,
                PrecoMedioCompra = 1000.00m,
                PrecoMercado = 1200.00m,
                VariacaoDiaria = 1.25m
            });

            portfolio.AddOrUpdate(new TituloRendaFixa
            {
                Nome = "CDB PREFIXADO",
                ValorInvestido = 10000.00m,
                TaxaAnual = 10.00m,
                DataAplicacao = new DateTime(2025, 5, 10), // ano / mês / dia
                DataVencimento = new DateTime(2026, 11, 10) // ano / mês / dia
            });


            // 1 Portfólio De Investimentos
            Console.WriteLine("=== Portfólio de Investimentos ===");
            var rentabilidade = new Rentabilidade(portfolio);
            var valortotal = rentabilidade.ValorTotalDoPortolio();
            Console.WriteLine($"\nValor Total: {valortotal:C2}");
            // Mostrar valor da média ponderada
            var mediaPonderada = rentabilidade.CalcularRentabilidadeMediaPonderada();
            Console.WriteLine($"Rentabilidade Média Ponderada: {mediaPonderada:f2}%");

            // 2 Renda Periódica Total
            Console.WriteLine("\n=== Renda Periódica Total ===");
            foreach (var gerador in portfolio.GetAll().OfType<IGeradorDeRenda>())
            {
                Console.WriteLine($"{((IAtivoFinanceiro)gerador).Nome}: Renda periódica = {gerador.CalcularRendaPeriodica():C2}");
            }

            var rendaPeriodicaTotal = portfolio.GetAll()
                .OfType<IGeradorDeRenda>()
                .Sum(r => r.CalcularRendaPeriodica());
            Console.WriteLine($"Total de Renda periódica mensal: {rendaPeriodicaTotal:C2}\n");

            // 3 Ativos próximos do vencimento
            Console.WriteLine("=== Ativos próximos do vencimento ===");
            Console.WriteLine("(próximos 180 dias)");

            var proximosVencimento = portfolio.GetAll()
                .Where(a => a is IAtivoComVencimento)
                .Select(a => new { Ativo = a, Venc = (IAtivoComVencimento)a })
                .Where(x =>
                {
                    var dias = x.Venc.DiasParaVencimento();
                    return dias >= 0 && dias <= 180;
                })
                .ToList();

            if (!proximosVencimento.Any())
            {
                Console.WriteLine("Nenhum ativo com vencimento nos próximos 180 dias.");
            }
            else
            {
                foreach (var item in proximosVencimento)
                {
                    Console.WriteLine($"{item.Ativo.Nome} - Vencimento: {item.Venc.DataVencimento:dd/MM/yyyy} (em {item.Venc.DiasParaVencimento()} dias).");
                }
            }

            // 3.5
            // Opcional, Pesquisando por interfaces
            Console.WriteLine("\n=== Ativos que geram renda periódica ===");
            var geradoresDeRenda = portfolio.FiltrarPor(a => a is IGeradorDeRenda);

            foreach (var item in geradoresDeRenda)
            {
                // Fazer o cast para acessar os métodos da interface
                var gerador = (IGeradorDeRenda)item;
                var nome = ((IAtivoFinanceiro)item).Nome;

                Console.WriteLine($"{nome} | Renda: {gerador.CalcularRendaPeriodica():C2} | Periodicidade: {gerador.Periodicidade}.");
            }
            string nomeParaBuscar = "PETROBRAS PN";

            // Busque o ativo pelo nome
            var ativo = portfolio.GetAll().FirstOrDefault(a => a.Nome == nomeParaBuscar);

            if (ativo != null)
            {
                Console.WriteLine($"Ativo encontrado pelo Nome: {ativo.Nome} - Preço: R$ {ativo.ValorAtual}.");
            }
            else
            {
                Console.WriteLine($"Ativo {nomeParaBuscar} não encontrado.");
            }

            // 4 Gerando relatório
            Relatorio.Gerar(portfolio);
        }

    }
}
