using Microsoft.Extensions.Configuration;
using System.IO;
using System;
using System.Threading.Tasks;
using Data_Access_Layer.Common.Repositories;
using Data_Access_Layer.Exceptions;
using Data_Access_Layer.Common.Models;
using Extention_Layer;

namespace Data_Access_Layer.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly IConfiguration _configuration;
        private string _path;
        public FileRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _path = _configuration.GetSection("PathFiles").Get<string>();
        }

        /// <summary>
        /// сохраняем
        /// </summary>
        public async Task Set(GroupDataDL item)
        {
            await JsonWorker.Set($"{_path}/{item.Id}.json", item);
        }

        /// <summary>
        /// забираем
        /// </summary>
        /// <returns>данные</returns>
        /// <exception cref="DAException">файл пустой</exceptEion>
        public async Task<GroupDataDL> Get(Guid id)
        {
            return JsonWorker.Get<GroupDataDL>($"{_path}/{id}.json"); ;
        }
    }
}
