using Business_Logic_Layer.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;

namespace Business_Logic_Layer.Extensions.Colors
{
    public class LineGradient
    {
        /// <summary>
        /// Линейный генератор цветов
        /// </summary>
        /// <param name="count">количество элементов</param>
        /// <param name="colors">начально заданный цвет</param>
        /// <returns>цвета</returns>
        /// <exception cref="DataValidException"></exception>
        public static IList<Color> GetColors(int count, IList<Color> colors)
        {
            List<Color> result = new List<Color>();
            var colorsNew = Check(count, colors);
            //находим % который занимает в градации между цветам i и i + 1 элементов colors, в сумме будет 100% * (count - 1)
            //при переходе через 100% выбираем другую группу colors (i + 1)
            //потеря точности из-за double, влияет?
            double shift = ((double)colorsNew.Count - 1) / (count - 1);
            double startShift = 0;
            for (var q = 0; q < colorsNew.Count - 1; q++)
            {
                for (var i = 0; i < count; i++)
                {
                    //есть ошибка в округлении в int, влияет? 
                    int r = (int)((colorsNew[q + 1].R - colorsNew[q].R) * startShift + colorsNew[q].R);
                    int g = (int)((colorsNew[q + 1].G - colorsNew[q].G) * startShift + colorsNew[q].G);
                    int b = (int)((colorsNew[q + 1].B - colorsNew[q].B) * startShift + colorsNew[q].B);
                    result.Add(Color.FromArgb(255, r, g, b));
                    startShift = startShift + shift;
                    if (Math.Round(startShift, 5) > 1)
                        break;
                }
                while (startShift > 1)
                {
                    startShift = startShift - 1;
                    if (startShift > 1)
                        q++;
                }
            }
            return result;
        }

        private static List<Color> Check(int count, IList<Color> colors)
        {
            List<Color> colorsNew = new List<Color>();
            if (count < 1)
                throw new DataValidException("Неверно заданное количество элементов");
            if (colors == null || colors.Count < 1)
                throw new DataValidException("Цвета не заданы");
            if (count == 1)
            {
                colorsNew.Add(colors[0]);
                return colorsNew;
            }
            if (count > 1 && colors.Count == 1)
            {
                colorsNew.Add(colors[0]);
                colorsNew.Add(Color.FromArgb(255, colors[0].R / 10, colors[0].G / 10, colors[0].B / 10));
            }
            else
                colorsNew = colors.ToList();

            return colorsNew;
        }
    }
}
