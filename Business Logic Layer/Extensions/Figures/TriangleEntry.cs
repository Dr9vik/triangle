using Business_Logic_Layer.Common.Models;
using Business_Logic_Layer.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business_Logic_Layer.Common.Extensions.Crossing
{
    public static class TriangleEntry
    {
        /// <summary>
        /// проверка на пересечение фигур
        /// </summary>
        /// <param name="elemOne">фигура</param>
        /// <param name="elemTwo">фигура</param>
        /// <returns>-1 нет пересечения; 0 есть пересечение; 1 elemTwo в elemOne</returns>
        /// <exception cref="DataValidException"></exception>
        public static int CheckElemByElem(this IList<Point2BL> elemOne, IList<Point2BL> elemTwo)
        {
            Check(elemOne);
            Check(elemTwo);
            int min = 0;
            int max = 0;
            List<int> param = new List<int>();

            for (var i = 0; i < elemTwo.Count; i++)
            {
                var re = CheckPointByELem(elemOne, elemTwo[i]);
                param.Add(re);
            }
            min = param.Min();
            max = param.Max();
            if (min > 0 && max > 0)
                return 1; //elemTwo в elemOne
            else if ((min < 0 && max < 0))
                return -1;//elemTwo не в elemOne
            else
                return 0;//elemTwo пересекает elemOne
        }

        /// <summary>
        /// входит ли точка в фигуру
        /// </summary>
        /// <param name="elem">фигура</param>
        /// <param name="point">точка</param>
        /// <returns>1 точка принадлежит елементу; 0 точка на стороне елемента; -1 точка не принадлежит елементу</returns>
        public static int CheckPointByELem(this IList<Point2BL> elem, Point2BL point)
        {
            //int a = (A.X - D.X) * (B.Y - A.Y) - (B.X - A.X) * (A.Y - D.Y);
            //int b = (B.X - D.X) * (C.Y - B.Y) - (C.X - B.X) * (B.Y - D.Y);
            //int c = (C.X - D.X) * (A.Y - C.Y) - (A.X - C.X) * (C.Y - D.Y);
            if (elem.Any(x => x.X == point.X && x.Y == point.Y))
                return 0;//точка на стороне елемента,в данном случае угол общий, по хорошему надо другой код

            int summ = 0;
            int a = (elem[0].X - point.X) * (elem[1].Y - elem[0].Y) - (elem[1].X - elem[0].X) * (elem[0].Y - point.Y);
            int b = (elem[1].X - point.X) * (elem[2].Y - elem[1].Y) - (elem[2].X - elem[1].X) * (elem[1].Y - point.Y);
            int c = (elem[2].X - point.X) * (elem[0].Y - elem[2].Y) - (elem[0].X - elem[2].X) * (elem[2].Y - point.Y);

            if (a > 0)
                summ++;
            else if (a < 0)
                summ--;

            if (b > 0)
                summ++;
            else if (b < 0)
                summ--;

            if (c > 0)
                summ++;
            else if (c < 0)
                summ--;

            summ = Math.Abs(summ);

            if (summ == 3)
                return 1; //точка принадлежит елементу
            else if (summ == 2)
                return 0;//точка на стороне елемента
            else
                return -1;//точка не принадлежит елементу
        }

        public static bool CheckLineByPoint(Point2BL first, Point2BL second, Point2BL three)
        {
            if ((first.X - three.X) * (second.Y - three.Y) == (first.Y - three.Y) * (second.X - three.X))
                return true;
            else
                return false;
        }

        private static bool Check(IList<Point2BL> elem)
        {
            //if ((x_3 - x_1) / (x_2 - x_1) == (y_3 - y_1) / (y_2 - y_1))
            if (elem.Count != 3)
                throw new DataValidException("Фигура являются не треугольниками");

            var x = elem.GroupBy(x => x.X).Count();
            if (x <= 1)
                throw new DataValidException("Фигура являются не треугольниками");

            x = elem.GroupBy(x => x.Y).Count();
            if (x <= 1)
                throw new DataValidException("Фигура являются не треугольниками");

            x = elem.GroupBy(x => (x.X, x.Y)).Count();
            if (x != 3)
                throw new DataValidException("Фигура являются не треугольниками");
            if (CheckLineByPoint(elem[0], elem[1], elem[2]))
                throw new DataValidException("Фигура являются не треугольниками");
            return true;
        }

    }
}
