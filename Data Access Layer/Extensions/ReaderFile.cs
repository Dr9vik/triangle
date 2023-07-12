using Data_Access_Layer.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Data_Access_Layer.Extensions
{
    public class ReaderFile
    {
        /// <summary>
        /// забираем данные из файла
        /// </summary>
        /// <param name="path">путь</param>
        /// <returns>масиив из массива x1 y1 x2 y2 x3 y3 .... xn yn</returns>
        public static async Task<IList<IList<int>>> Get(string path)
        {
            IList<IList<int>> result = new List<IList<int>>();
            StreamReader reader = new StreamReader(path);
            try
            {
                string line;
                while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) != null)
                {
                    List<int> ints = new List<int>();
                    var re = line.Split(' ');
                    foreach (var item in re)
                    {
                        ints.Add(int.Parse(item));
                    }
                    result.Add(ints);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { reader.Close(); reader.Dispose(); }
            return result;
        }
    }
}
