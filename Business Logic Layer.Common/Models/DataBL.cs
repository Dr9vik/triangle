using System.Collections.Generic;
using System.Drawing;

namespace Business_Logic_Layer.Common.Models
{
    public class DataBL
    {
        public int ZIndex { get; set; }
        public Color Color { get; set; }
        public IList<Point2BL> Points { get; set; }
    }
}
