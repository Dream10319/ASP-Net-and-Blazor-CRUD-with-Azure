using Simple_FrontEnd.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simple_FrontEnd.Services
{
    public interface IParticipantsService
    {
        Participants participant { get; }
        Task Register(Participants model);
        Task<IList<Participants>> GetAll();
        Task<Participants> GetById(string id);
        Task Update(string id, Participants model);
        Task Delete(string id);
    }
 
    public class ParticipantsService : IParticipantsService
    {
        private IHttpService _httpService;

        public Participants participant { get; private set; }

        public ParticipantsService(
            IHttpService httpService
        ) {
            _httpService = httpService;
        }

        public async Task Register(Participants model)
        {
            await _httpService.Post("/api/Participants", model);
        }

        public async Task<IList<Participants>> GetAll()
        {
            return await _httpService.Get<IList<Participants>>("/api/Participants");
        }

        public async Task<Participants> GetById(string id)
        {
            return await _httpService.Get<Participants>($"/api/Participants/{id}");
        }

        public async Task Update(string id, Participants model)
        {
            await _httpService.Put($"/api/Participants/{id}", model);

        }

        public async Task Delete(string id)
        {
            await _httpService.Delete($"/api/Participants/{id}");
        }
    }
}