using Microsoft.Extensions.Configuration;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Data_Access_Layer.Common.Repositories;
using Data_Access_Layer.Exceptions;
using System.Linq;
using Data_Access_Layer.Common.Models;
using Data_Access_Layer.Extensions;

namespace Data_Access_Layer.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly IConfiguration _configuration;
        public FileRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Забираем данные
        /// </summary>
        /// <returns>данные</returns>
        /// <exception cref="DAException">файл пустой</exceptEion>
        public async Task<IList<DataDL>> Get()
        {
            var path = _configuration.GetSection("DB").GetSection("Path").Get<string>();
            var result = await ReaderFile.Get(path).ConfigureAwait(false);
            Check(result);
            IList<DataDL> result2 = new List<DataDL>();
            for (var i = 1; i <= result[0][0]; i++)
            {
                List<Point2DL> point = new List<Point2DL>();
                for (var q = 0; q < result[i].Count; q = q + 2)
                    point.Add(new Point2DL() { X = result[i][q], Y = result[i][q + 1] });
                result2.Add(new DataDL() { Points = point });
            }
            return result2;
        }

        /// <summary>
        /// проверяем данные на валидность
        /// </summary>
        /// <param name="items">данные</param>
        /// <exception cref="DAException"></exception>
        private void Check(IList<IList<int>> items)
        {
            if (items != null && items.Count == 0)
                return;
            if (items == null
                || items.Count == 1
                || items[0].Count != 1
                || (items[0][0] < 0 && items[0][0] > 1000)
                || items[0][0] > items.Count() - 1)
                throw new DAException("Файл повреждён");

            for (var i = 1; i < items.Count; i++)
            {
                if (items[i].Count % 2 != 0 && items[i].Count != 6)
                    throw new DAException("Файл повреждён");
                for (var q = 0; q < items[i].Count; q++)
                {
                    if (items[i][q] < 0 || items[i][q] > 1000)
                        throw new DAException("Файл повреждён");

                }
            }
        }
    }
}
