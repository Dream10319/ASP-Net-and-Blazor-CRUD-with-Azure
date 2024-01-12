using Simple_FrontEnd.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simple_FrontEnd.Services
{
    public interface IClientsService
    {
        Custumers custumers { get; }
        Task Register(Custumers model);
        Task<IList<Custumers>> GetAll();
        Task<Custumers> GetById(string id);
        Task Update(string id, Custumers model);
        Task Delete(string id);
    }
 
    public class ClientsService : IClientsService
    {
        private IHttpService _httpService;

        public Custumers custumers { get; private set; }

        public ClientsService(
            IHttpService httpService
        ) {
            _httpService = httpService;
        }

        public async Task Register(Custumers model)
        {
            await _httpService.Post("/api/Custumers", model);
        }

        public async Task<IList<Custumers>> GetAll()
        {
            return await _httpService.Get<IList<Custumers>>("/api/Custumers");
        }

        public async Task<Custumers> GetById(string id)
        {
            return await _httpService.Get<Custumers>($"/api/Custumers/{id}");
        }

        public async Task Update(string id, Custumers model)
        {
            await _httpService.Put($"/api/Custumers/{id}", model);

        }

        public async Task Delete(string id)
        {
            await _httpService.Delete($"/api/Custumers/{id}");
        }
    }
}