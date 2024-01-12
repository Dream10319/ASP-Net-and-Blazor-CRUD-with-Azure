using Simple_FrontEnd.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simple_FrontEnd.Services
{
    public interface IPaymentTypeService
    {
        PaymentType PaymentType { get; }
        Task Register(PaymentType model);
        Task<IList<PaymentType>> GetAll();
        Task<PaymentType> GetById(string id);
        Task Update(string id, PaymentType model);
        Task Delete(string id);
    }
 
    public class PaymentTypeService : IPaymentTypeService
    {
        private IHttpService _httpService;

        public PaymentType PaymentType { get; private set; }

        public PaymentTypeService(
            IHttpService httpService
        ) {
            _httpService = httpService;
        }

        public async Task Register(PaymentType model)
        {
            await _httpService.Post("/api/PaymentType", model);
        }

        public async Task<IList<PaymentType>> GetAll()
        {
            return await _httpService.Get<IList<PaymentType>>("/api/PaymentType");
        }

        public async Task<PaymentType> GetById(string id)
        {
            return await _httpService.Get<PaymentType>($"/api/PaymentType/{id}");
        }

        public async Task Update(string id, PaymentType model)
        {
            await _httpService.Put($"/api/PaymentType/{id}", model);

        }

        public async Task Delete(string id)
        {
            await _httpService.Delete($"/api/PaymentType/{id}");
        }
    }
}