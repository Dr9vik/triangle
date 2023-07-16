using System.Collections.Generic;

namespace Data_Access_Layer.Common.Models
{
    public class DataDL
    {
        public int ZIndex { get; set; }
        public IList<Point2DL> Points { get; set; }
    }
}
