using Desafio_Mini_Sistema_de_Portfólio_de_Investimentos.Ativos;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Desafio_Mini_Sistema_de_Portfólio_de_Investimentos.Repository
{

    public class Portfolio<T> : IRepository<T> where T : IAtivoFinanceiro
    {
        private Dictionary<string, T> TabelaAtivosFinanceiros { get; set; }

        // Implementação da interface não-genérica (compatibilidade)
        public void AddOrUpdate(T ativo)
        {
            if (ativo == null) throw new ArgumentNullException(nameof(ativo));
            
            if (ativo is not T casted) throw new ArgumentException($"Ativo deve ser do tipo {typeof(T).Name}", nameof(ativo));

            // Usando GeType()
            Type tipoObjeto = this.GetType();

            // usando GetProperties(), para conseguir ler propriedades privadas da classe
            PropertyInfo[] propriedades = tipoObjeto.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (PropertyInfo prop in propriedades)
            {
                Type tipoPropriedade = prop.PropertyType;

                // Usando GetInterfaces()
                bool ehRepositorio = tipoPropriedade.GetInterfaces().Any(i => i == typeof(IDictionary));

                if (ehRepositorio)
                {
                    object? valorAtual = prop.GetValue(this);

                    if (valorAtual == null)
                    {
                        object novoRepositorio = Activator.CreateInstance(tipoPropriedade);
                        prop.SetValue(this, novoRepositorio);

                        valorAtual  = prop.GetValue(this);
                    }

                    if (valorAtual is IDictionary dicionario)
                    {
                        dicionario[ativo.Nome] = casted;
                        //Console.WriteLine($"[Reflection] Ativo '{ativo.Nome}' adicionado/atualizado com sucesso.");
                    }

                    break;
                }
            }
        }

        // S – Single Responsibility - Principle Princípios SOLID
        public T GetByNome(string chave)
        {
            GarantirRepositorio();
            TabelaAtivosFinanceiros.TryGetValue(chave, out var ativo);
            return ativo;
        }

        // S – Single Responsibility Principle - Princípios SOLID
        public IEnumerable<T> GetAll()
        {
            GarantirRepositorio();
            return TabelaAtivosFinanceiros.Values;
        }
        // S – Single Responsibility Principle - Princípios SOLID
        private void GarantirRepositorio()
        {
            if (TabelaAtivosFinanceiros == null)
            {
                TabelaAtivosFinanceiros = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);
            }
        }

        public IEnumerable<T> FiltrarPor(Func<T, bool> predicado)
        {
            if (predicado == null)
                throw new ArgumentNullException(nameof(predicado));

            return TabelaAtivosFinanceiros.Values.Where(predicado);
        }
    }

}


