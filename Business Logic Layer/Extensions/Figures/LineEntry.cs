using Business_Logic_Layer.Common.Models;
using Business_Logic_Layer.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business_Logic_Layer.Common.Extensions.Crossing
{
    public static class LineEntry
    {
        /// <summary>
        /// проверка нахождение точки на линии
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="three">точка</param>
        /// <returns>true - находиться, false - не находиться</returns>
        public static bool CheckLineByPoint(Point2BL firstPointLine, Point2BL secondPointLine, Point2BL point)
        {
            return (firstPointLine.X - point.X) * (secondPointLine.Y - point.Y) == (firstPointLine.Y - point.Y) * (secondPointLine.X - point.X);
        }
    }
}
