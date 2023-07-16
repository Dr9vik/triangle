using Business_Logic_Layer.Common.Models;
using Business_Logic_Layer.Exceptions;
using Business_Logic_Layer.Extensions.Colors;
using System.Drawing;
using System.Linq;

namespace Business_Logic_Layer.Extensions
{
    public static class ColorFigures
    {
        /// <summary>
        /// добавляетт цвет фигурам
        /// </summary>
        /// <param name="item">данные</param>
        /// <param name="colors">требуемый цвет, если цвет в массиве один, то будет построен градиент на основе более темного оттенка
        /// если цвета 2 или более, градиент будет строится относительно их</param>
        /// <returns>данные</returns>
        /// <returns>данные с цветом></returns>
        public static GroupDataBL Add(GroupDataBL item, params Color[] colors)
        {
            CheckAdd(item, colors);

            var zIndexGroup = item.Datas.GroupBy(x => x.ZIndex).OrderBy(x => x.Key).ToList();
            var colorGroup = LineGradient.GetColors(zIndexGroup.Count, colors);

            int i = 0;
            foreach (var items in zIndexGroup)
            {
                foreach (var data in items)
                {
                    data.Color = colorGroup[i];
                }
                i++;
            }
            return item;
        }
        private static void CheckAdd(GroupDataBL item, params Color[] colors)
        {
            if (item == null)
                throw new DataValidException("GroupDataBL null");
            if (item.Datas == null || !item.Datas.Any())
                throw new DataValidException("item.Datas or 0");
            if (colors == null || colors.Length == 0)
                throw new DataValidException("Отсутствуют цвета");
        }
    }
}
