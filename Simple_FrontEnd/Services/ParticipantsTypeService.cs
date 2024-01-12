using Simple_FrontEnd.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simple_FrontEnd.Services
{
    public interface IParticipantsTypeService
    {
        ParticipantsType ParticipantsType { get; }
        Task Register(ParticipantsType model);
        Task<IList<ParticipantsType>> GetAll();
        Task<ParticipantsType> GetById(string id);
        Task Update(string id, ParticipantsType model);
        Task Delete(string id);
    }
 
    public class ParticipantsTypeService : IParticipantsTypeService
    {
        private IHttpService _httpService;

        public ParticipantsType ParticipantsType { get; private set; }

        public ParticipantsTypeService(
            IHttpService httpService
        ) {
            _httpService = httpService;
        }

        public async Task Register(ParticipantsType model)
        {
            await _httpService.Post("/api/ParticipantsType", model);
        }

        public async Task<IList<ParticipantsType>> GetAll()
        {
            return await _httpService.Get<IList<ParticipantsType>>("/api/ParticipantsType");
        }

        public async Task<ParticipantsType> GetById(string id)
        {
            return await _httpService.Get<ParticipantsType>($"/api/ParticipantsType/{id}");
        }

        public async Task Update(string id, ParticipantsType model)
        {
            await _httpService.Put($"/api/ParticipantsType/{id}", model);

        }

        public async Task Delete(string id)
        {
            await _httpService.Delete($"/api/ParticipantsType/{id}");
        }
    }
}