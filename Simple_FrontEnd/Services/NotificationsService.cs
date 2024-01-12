using Simple_FrontEnd.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simple_FrontEnd.Services
{
    public interface INotificationsService
    {
        NotificationType notificationType { get; }
        Task Register(NotificationType model);
        Task<IList<NotificationType>> GetAll();
        Task<NotificationType> GetById(string id);
        Task Update(string id, NotificationType model);
        Task Delete(string id);
    }
 
    public class NotificationsService : INotificationsService
    {
        private IHttpService _httpService;

        public NotificationType notificationType { get; private set; }

        public NotificationsService(
            IHttpService httpService
        ) {
            _httpService = httpService;
        }

        public async Task Register(NotificationType model)
        {
            await _httpService.Post("/api/NotificationType", model);
        }

        public async Task<IList<NotificationType>> GetAll()
        {
            return await _httpService.Get<IList<NotificationType>>("/api/NotificationType");
        }

        public async Task<NotificationType> GetById(string id)
        {
            return await _httpService.Get<NotificationType>($"/api/NotificationType/{id}");
        }

        public async Task Update(string id, NotificationType model)
        {
            await _httpService.Put($"/api/NotificationType/{id}", model);

        }

        public async Task Delete(string id)
        {
            await _httpService.Delete($"/api/NotificationType/{id}");
        }
    }
}