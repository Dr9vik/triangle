using Business_Logic_Layer.Common.Models;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Common.Services
{
    public interface IDataService
    {
        Task<IList<DataBL>> Get(params Color[] colors);
    }
}
