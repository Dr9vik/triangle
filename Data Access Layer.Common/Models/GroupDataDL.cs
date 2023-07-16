using System;
using System.Collections.Generic;

namespace Data_Access_Layer.Common.Models
{
    public class GroupDataDL
    {
        public Guid Id { get; set; }
        public IList<DataDL> Datas { get; set; }
    }
}
