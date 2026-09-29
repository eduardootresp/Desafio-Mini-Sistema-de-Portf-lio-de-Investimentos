using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Desafio_Mini_Sistema_de_Portfólio_de_Investimentos.GeradorDeRelatorio
{
    // Gerador de Relatório
    public static class Relatorio
    {
        // O – Open/Closed Principle - Princípios SOLID
        public static void Gerar(IRepository<IAtivoFinanceiro> portfolio)
        {
            Console.WriteLine("\n=== Relatório Dinâmico (via Reflection) ===");

            foreach (var ativo in portfolio.GetAll())
            {
                var tipo = ativo.GetType();
                Console.WriteLine($"\nClasse: {ativo.GetType().Name}");

                var propriedades = ativo.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var prop in propriedades)
                {
                    object valorObj;
                    try { valorObj = prop.GetValue(ativo) ?? "<null>"; }
                    catch { valorObj = "<erro ao obter>"; }

                    if (valorObj is DateTime dt)
                    {
                        Console.WriteLine($"{prop.Name}: {dt:dd/MM/yyyy}");
                    }
                    else if (valorObj is decimal dec)
                    {
                        // Detectar VariacaoDiaria pelo nome e formatar como porcentagem
                        if (string.Equals(prop.Name, "VariacaoDiaria", StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(prop.Name, "TaxaAdministracao", StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(prop.Name, "TaxaAnual", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"{prop.Name}: {dec:F2}%");
                        }
                        else
                        {
                            Console.WriteLine($"{prop.Name}: {dec:C2}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{prop.Name}: {valorObj}");
                    }
                }
                Console.WriteLine($"Rentabilidade: {ativo.CalcularRentabilidade():p2}");

                // L – Liskov Substitution Principle - Pincípios SOLID
                var interfaces = ativo.GetType().GetInterfaces();
                foreach (var inter in interfaces)
                {
                    Console.WriteLine($"Implementa interface: {inter.Name}");
                }

                // D – Dependency Inversion Principle - Pincípios SOLID
                // Detectar interfaces em tempo de execução (sem checar tipos concretos)
                if (typeof(IAtivoComVencimento).IsAssignableFrom(tipo))
                {
                    var propVenc = tipo.GetProperty(nameof(IAtivoComVencimento.DataVencimento));
                    var metodoDias = tipo.GetMethod(nameof(IAtivoComVencimento.DiasParaVencimento));
                    var valVenc = propVenc?.GetValue(ativo) ?? "<indisponível>";
                    var dias = metodoDias?.Invoke(ativo, null) ?? "<indisponível>";
                    Console.WriteLine($"\n[IAtivoComVencimento] DataVencimento: {valVenc:dd/MM/yyyy} | DiasParaVencimento: {dias}.");
                }

                if (typeof(IGeradorDeRenda).IsAssignableFrom(tipo))
                {
                    var metodoRendaPeriodica = tipo.GetMethod(nameof(IGeradorDeRenda.CalcularRendaPeriodica));
                    var propPeriodicidade = tipo.GetProperty(nameof(IGeradorDeRenda.Periodicidade));
                    var renda = metodoRendaPeriodica != null ? metodoRendaPeriodica.Invoke(ativo, null) : "<indisponível>";
                    var periodicidade = propPeriodicidade?.GetValue(ativo) ?? "<indisponível>";
                    Console.WriteLine($"\n[IGeradorDeRenda] Renda periódica: {renda:c2} | Periodicidade: {periodicidade}.");
                }

                if (typeof(IAtivoNegociavel).IsAssignableFrom(tipo))
                {
                    var propPreco = tipo.GetProperty(nameof(IAtivoNegociavel.PrecoMercado));
                    var propVariacao = tipo.GetProperty(nameof(IAtivoNegociavel.VariacaoDiaria));
                    var preco = propPreco?.GetValue(ativo) ?? "<indisponível>";
                    var variacao = propVariacao?.GetValue(ativo) ?? "<indisponível>";
                    Console.WriteLine($"\n[IAtivoNegociavel] Preço mercado: {preco:c2} | Variação diária: {variacao}%.");
                }

                Console.WriteLine("\n---------------------------------------\n");
            }
        }
    }
}
