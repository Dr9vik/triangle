using Data_Access_Layer.Common.Models;
using System;
using System.Threading.Tasks;

namespace Data_Access_Layer.Common.Repositories
{
    public interface IRepository<T>
    {
        Task Set(T item);
        Task<T> Get(Guid Id);
    }
}
