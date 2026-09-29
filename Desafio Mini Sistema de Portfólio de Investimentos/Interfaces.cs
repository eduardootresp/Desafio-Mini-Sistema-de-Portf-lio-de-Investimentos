using Desafio_Mini_Sistema_de_Portfólio_de_Investimentos.Ativos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio_Mini_Sistema_de_Portfólio_de_Investimentos
{
    /** 
    *Classe	            Interfaces que deve implementar*
    Acao	            IAtivoFinanceiro + IGeradorDeRenda + IAtivoNegociavel
    TituloRendaFixa	    IAtivoFinanceiro + IAtivoComVencimento
    FundoInvestimento	IAtivoFinanceiro + IGeradorDeRenda
     * 
     Opcional (desafio extra):

    Implemente também a classe:

    Cripto

    implementando:

    IAtivoFinanceiro + IAtivoNegociavel

     */


    // Interface base mínima: todo ativo financeiro deve implementar
    public interface IAtivoFinanceiro
    {
        string Nome { get; }
        decimal ValorInvestido { get; }
        decimal ValorAtual { get; }

        decimal CalcularRentabilidade();
    }

    // Interface específica: ativos que possuem data de vencimento
    public interface IAtivoComVencimento
    {
        DateTime DataVencimento { get; }
        int DiasParaVencimento();
    }

    // Interface específica: ativos que geram renda periódica
    // como dividendos, distribuições etc.
    public interface IGeradorDeRenda
    {
       decimal CalcularRendaPeriodica();
        string Periodicidade { get; } // Ex.: "Mensal", "Trimestral", "Anual" 
    }

    // Interface específica: ativos negociáveis em mercado
    public interface IAtivoNegociavel
    {
        decimal PrecoMercado { get; }
        decimal VariacaoDiaria { get; } // percentual de variação do dia
    }

    // "Dependa de abstrações, não de implementações." — Princípio da Inversão de Dependência(D do SOLID)
    public interface IRepository<T> where T : IAtivoFinanceiro
    {
        void AddOrUpdate(T item);
        T GetByNome(string chave);
        IEnumerable<T> GetAll();

        IEnumerable<T> FiltrarPor(Func<T, bool> predicado);
    }
}
