using Simple_FrontEnd.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simple_FrontEnd.Services
{
    public interface ILogsService
    {
        Logs logs { get; }
        Task<IList<Logs>> GetAll();
        Task<Logs> GetById(string id);
        Task Delete(string id);
    }
 
    public class LogsService : ILogsService
    {
        private IHttpService _httpService;

        public Logs logs { get; private set; }

        public LogsService(
            IHttpService httpService
        ) {
            _httpService = httpService;
        }

        public async Task<IList<Logs>> GetAll()
        {
            return await _httpService.Get<IList<Logs>>("/api/Logs");
        }

        public async Task<Logs> GetById(string id)
        {
            return await _httpService.Get<Logs>($"/api/Logs/{id}");
        }

        public async Task Delete(string id)
        {
            await _httpService.Delete($"/api/Logs/{id}");
        }
    }
}