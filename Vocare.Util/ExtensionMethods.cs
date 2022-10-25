using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Vocare.Util
{
    public static partial class ExtensionMethods
    {
        /// <summary>
        /// Verifica sestring é Null ou Empty (string.IsNullOrEmpty)
        /// </summary>
        /// <param name="Valor para verificação"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string o) => string.IsNullOrEmpty(o);

        /// <summary>
        /// Converte o valor do objto para INT, caso não consiga converter, retorna zero.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static int ParseIntOrDefault(this object o)
        {
            if (o == null) return 0;
            int TryedValue;
            return int.TryParse(o.ToString(), out TryedValue) ? TryedValue : 0;
        }

        /// <summary>
        /// Converte uma lista de Int para lista de string
        /// </summary>
        /// <param name="lst">Lista de Inteiros (Int32)</param>
        /// <returns>Lista de string</returns>
        public static List<string> ToListString(this List<int> lst) => lst.Select(x => x.ToString()).ToList();

        /// <summary>
        /// Converte uma lista de String para lista de Int
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        public static List<int> ToListInt(this List<string> lst) => lst.Select(x => x.ParseIntOrDefault()).ToList();

        /// <summary>
        /// Retorna uma coleção com registros distintos
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Converte uma string com valores para uma lista de strings a partir de um separador (,)
        /// </summary>
        /// <param name="o"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static List<string> ToListString(this string o, string s = ",") => o.Coalesce("").Split(s.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

        /// <summary>
        /// Retorna o primeiro valor diferente de Null e DBNull
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="valores"></param>
        /// <returns></returns>
        public static T Coalesce<T>(this T o, params T[] valores)
        {
            if (o != null || typeof(T) == DBNull.Value.GetType() && o as DBNull != DBNull.Value)
            {
                return o;
            }
            else
            {
                if (valores == null)
                    return o;

                if (valores.Length == 0)
                    return o;

                T Valor = valores[0];
                List<T> array = valores.ToList();
                array.RemoveAt(0);

                return Valor.Coalesce(array.ToArray());
            }
        }

        /// <summary>
        /// Verifica se o filtro é valido
        /// </summary>
        /// <param name="filter"></param>
        public static void ValidateFilter<T>(this string filter)
        {
            if (!string.IsNullOrEmpty(filter))
            {
                string[] filterArr = filter.Split('\u003A');
                if (filterArr.Length < 1)
                    throw new ApplicationException("Filtro inválido");

                PropertyInfo[] props = typeof(T).GetProperties();
                if (!props.Any(prop => prop.Name.ToUpper() == filterArr[0].ToUpper()))
                    throw new ApplicationException("Filtro inválido");
            }
        }

        /// <summary>
        /// Verifica se o filtro é valido
        /// </summary>
        /// <param name="filter"></param>
        public static void ValidateOrder<T>(this string filter)
        {
            if (!string.IsNullOrEmpty(filter))
            {
                string[] filterArr = filter.Split('\u003A');
                if (filterArr.Length < 1)
                    throw new ApplicationException("Ordem inválida");

                PropertyInfo[] props = typeof(T).GetProperties();
                if (!props.Any(prop => prop.Name.ToUpper() == filterArr[0].ToUpper()))
                    throw new ApplicationException("Ordem inválida");

                if (filterArr[1].ToUpper() != "ASC" && filterArr[1].ToUpper() != "DESC")
                    throw new ApplicationException("Ordem inválida");
            }
        }
    }
}
