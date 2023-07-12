using Data_Access_Layer.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data_Access_Layer.Common.Repositories
{
    public interface IFileRepository : IRepository<IList<DataDL>>
    {
    }
}
