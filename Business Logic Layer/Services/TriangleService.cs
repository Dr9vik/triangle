using Business_Logic_Layer.Common.Extensions.Crossing;
using Business_Logic_Layer.Common.Models;
using Business_Logic_Layer.Common.Services;
using Business_Logic_Layer.Exceptions;
using Business_Logic_Layer.Extensions.Colors;
using Data_Access_Layer.Common.Models;
using Data_Access_Layer.Common.Repositories;
using Data_Access_Layer.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Services
{
    /// <summary>
    /// сервис
    /// </summary>
    public class TriangleService : ITriangleService
    {
        private readonly IConfiguration _configuration;
        private readonly IFileRepository _repository;
        public TriangleService(IFileRepository repository, IConfiguration configuration)
        {
            _configuration = configuration;
            _repository = repository;
        }

        /// <summary>
        /// сервис для сохранения данных
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<GroupDataBL> Set(GroupDataBLCreate item)
        {
            CheckInputData(item);
            CheckCrossing(item.Datas);

            GroupDataDL data = new GroupDataDL();
            data.Id = Guid.NewGuid();
            data.Datas = item.Datas.Select(x =>
                new DataDL() { ZIndex = x.ZIndex, Points = x.Points.Select(z => new Point2DL() { X = z.X, Y = z.Y }).ToList() }).ToList();

            await _repository.Set(data).ConfigureAwait(false);

            //мапер лень использовать
            return new GroupDataBL()
            {
                Id = data.Id,
                Datas = item.Datas.Select(x =>
                new DataBL() { ZIndex = x.ZIndex, Points = x.Points.Select(z => new Point2BL() { X = z.X, Y = z.Y }).ToList() }).ToList()
            };
        }


        /// <summary>
        /// Сервис для получения данных
        /// </summary>
        /// <param name="id"></param>
        /// <param name="colors">треубемый цвет, если цвет в массиве один, то будет построен градиент на основе более темного оттенка
        /// если цвета 2 или более, градиент будет строится относительно их</param>
        /// <returns>данные</returns>
        /// <exception cref="DataValidException"></exception>
        public async Task<GroupDataBL> Get(Guid id, params Color[] colors)
        {
            if (colors.Length == 0)
                throw new DataValidException("Отсутствуют цвета");

            GroupDataBL result = new GroupDataBL();
            result.Id = id;

            var datas = await _repository.Get(id).ConfigureAwait(false);
            var datasMap = datas.Datas.Select(x => 
                new DataBL() { ZIndex = x.ZIndex, Points = x.Points.Select(z => new Point2BL() { X = z.X, Y = z.Y }).ToList() }).ToList();

            var zIndexGroup = datasMap.GroupBy(x => x.ZIndex).OrderBy(x => x.Key).ToList();
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
            result.Datas = datasMap;
            return result;
        }

        /// <summary>
        /// проверка на пересечение фигур
        /// по хорошему надо проверять такое при загрузке данных в систему, еще до вызова данных из репозитория
        /// но данные в файле, лень думать
        /// </summary>
        /// <param name="item">данные с репозитория</param>
        private void CheckCrossing(IList<DataBL> items)
        {
            for (var i = 0; i < items.Count - 1; i++)
            {
                for (var q = i + 1; q < items.Count; q++)
                {
                    var re = items[i].Points.CheckElemByElem(items[q].Points);
                    var re2 = items[q].Points.CheckElemByElem(items[i].Points);
                    if (re == 0 || re2 == 0)
                        throw new DataValidException("Фигуры пересекаются");
                    if (re > re2)
                        items[q].ZIndex++;
                    else if (re < re2)
                        items[i].ZIndex++;
                }
            }
        }

        /// <summary>
        /// проверяем данные на валидность
        /// </summary>
        /// <param name="items">данные</param>
        /// <exception cref="DAException"></exception>
        private void CheckInputData(GroupDataBLCreate item)
        {
            if (item == null)
                throw new DataValidException("Данные отсутствуют");

            if (item.Datas == null || item.Datas.Count == 0)
                throw new DataValidException("Данные отсутствуют");

            if (item.CountDatas != item.Datas.Count)
                throw new DataValidException("Количество данных не соответствует");

            if (item.Datas.Count > 1000)
                throw new DataValidException("> 1000");

            for (var i = 0; i < item.Datas.Count; i++)
            {
                if (item.Datas[i].Points.Count != 3)
                    throw new DataValidException("Данные не треугольник");
                for (var q = 0; q < item.Datas[i].Points.Count; q++)
                {
                    if (0 > item.Datas[i].Points[q].X || 1000 < item.Datas[i].Points[q].X)
                        throw new DataValidException("Выход за пределы");
                    if (0 > item.Datas[i].Points[q].Y || 1000 < item.Datas[i].Points[q].Y)
                        throw new DataValidException("Выход за пределы");
                }
            }
        }

    }
}

