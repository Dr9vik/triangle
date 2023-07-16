using Business_Logic_Layer.Common.Models;
using Data_Access_Layer.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Extensions
{
    public static class FileToGroupData
    {
        public static async Task<GroupDataBLCreate> Map(string path)
        {
            var result = new GroupDataBLCreate() { Datas = new List<DataBL>() };
            var datas = await ReadFile.Read(path).ConfigureAwait(false);

            result.CountDatas = datas[0][0];

            for (var i = 1; i < datas.Count; i++)
            {
                List<Point2BL> point = new List<Point2BL>();
                for (var q = 0; q < datas[i].Count; q = q + 2)
                    point.Add(new Point2BL() { X = datas[i][q], Y = datas[i][q + 1] });
                result.Datas.Add(new DataBL() { Points = point });
            }
            return result;
        }
    }
}
