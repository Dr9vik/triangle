using Business_Logic_Layer.Common.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Common.Services
{
    public interface IFigureService
    {
        Task<GroupDataBL> Set(GroupDataBLCreate item);
        Task<GroupDataBL> Get(Guid id, params Color[] colors);
    }
}
