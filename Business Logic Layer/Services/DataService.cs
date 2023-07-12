using Business_Logic_Layer.Common.Extensions.Crossing;
using Business_Logic_Layer.Common.Models;
using Business_Logic_Layer.Common.Services;
using Business_Logic_Layer.Exceptions;
using Business_Logic_Layer.Extensions.Colors;
using Data_Access_Layer.Common.Models;
using Data_Access_Layer.Common.Repositories;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Services
{
    /// <summary>
    /// сервис
    /// </summary>
    public class DataService : IDataService
    {
        private readonly IFileRepository _repository;
        public DataService(IFileRepository repository)
        {
            _repository = repository;
        }


        /// <summary>
        /// Сервис для выгрузки точек для построения треугольников и цвет каждого треугольника
        /// </summary>
        /// <param name="colors">треубемый цвет, если цвет в массиве один, то будет построен градиент на основе более темного оттенка
        /// если цвета 2 или более, градиент будет строится относительно их</param>
        /// <returns>данные</returns>
        /// <exception cref="DataValidException"></exception>
        public async Task<IList<DataBL>> Get(Color[] colors)
        {
            if (colors.Length == 0)
                throw new DataValidException("Отсутствую цвета");
            var data = await _repository.Get().ConfigureAwait(false);
            if (!data.Any())
                throw new DataValidException("Файл данных пуст");

            var dataNew = CheckCrossing(data);
            var zIndexGroup = dataNew.GroupBy(x => x.ZIndex).OrderBy(x=>x.Key).ToList();
            var colorGroup = LineGradient.GetColors(zIndexGroup.Count, colors);

            int i = 0;
            foreach (var items in zIndexGroup)
            {
                foreach (var item in items)
                {
                    item.Color = colorGroup[i];
                }
                i++;
            }
            return dataNew;
        }

        /// <summary>
        /// проверка на пересечение фигур
        /// по хорошему надо проверять такое при загрузке данных в систему, еще до вызова данных из репозитория
        /// но данные в файле, лень думать
        /// </summary>
        /// <param name="item">данные с репозитория</param>
        private IList<DataBL> CheckCrossing(IList<DataDL> items)
        {
            var results = items.Select(x => new DataBL() { ZIndex = 0, Points = x.Points.Select(z => new Point2BL() { X = z.X, Y = z.Y }).ToList() }).ToList();

            for (var i = 0; i < results.Count - 1; i++)
            {
                for (var q = i + 1; q < results.Count; q++)
                {
                    var re = results[i].Points.CheckElemByElem(results[q].Points);
                    var re2 = results[q].Points.CheckElemByElem(results[i].Points);
                    if (re == 0 || re2 == 0)
                        throw new DataValidException("Фигуры пресекаются");
                    if (re > re2)
                        results[q].ZIndex++;
                    else if (re < re2)
                        results[i].ZIndex++;
                }
            }
            return results;
        }

    }
}

