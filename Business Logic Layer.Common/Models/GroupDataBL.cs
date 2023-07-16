using System;
using System.Collections.Generic;

namespace Business_Logic_Layer.Common.Models
{
    public class GroupDataBL
    {
        public Guid Id { get; set; }
        public IList<DataBL> Datas { get; set; }
    }

    public class GroupDataBLCreate
    {
        public int CountDatas { get; set; }
        public IList<DataBL> Datas { get; set; }
    }

    public class GroupDataBLUpdate : GroupDataBLCreate
    {
        public Guid Id { get; set; }
    }
}
