using Simple_FrontEnd.Models;
using Simple_FrontEnd.Models.Account;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simple_FrontEnd.Services
{
    public interface IAccountService
    {
        User User { get; }
        Task Initialize();
        Task Login(Login model);
        Task Logout();
        Task Register(Users model);
        Task<IList<Users>> GetAll();
        Task<Users> GetById(string id);
        Task Update(string id, Users model);
        Task Delete(string id);
    }

    public class AccountService : IAccountService
    {
        private IHttpService _httpService;
        private NavigationManager _navigationManager;
        private ILocalStorageService _localStorageService;
        private string _userKey = "user";

        public User User { get; private set; }

        public AccountService(
            IHttpService httpService,
            NavigationManager navigationManager,
            ILocalStorageService localStorageService
        ) {
            _httpService = httpService;
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;
        }

        public async Task Initialize()
        {
            User = await _localStorageService.GetItem<User>(_userKey);
        }

        public async Task Login(Login model)
        {
            User = await _httpService.Post<User>("/api/Auth", model);
            await _localStorageService.SetItem(_userKey, User);
        }

        public async Task Logout()
        {
            User = null;
            await _localStorageService.RemoveItem(_userKey);
            _navigationManager.NavigateTo("account/login");
        }

        public async Task Register(Users model)
        {
            await _httpService.Post("/api/Users", model);
        }

        public async Task<IList<Users>> GetAll()
        {
            return await _httpService.Get<IList<Users>>("/api/Users");
        }

        public async Task<Users> GetById(string id)
        {
            return await _httpService.Get<Users>($"/api/Users/{id}");
        }

        public async Task Update(string id, Users model)
        {
            await _httpService.Put($"/api/Users/{id}", model);

            // update stored user if the logged in user updated their own record
            if (id == User.UserId.ToString()) 
            {
                // update local storage
                User.UserId = model.User_Id;
                User.PartId = model.Part_Id;
                User.FirstName = model.FirstName;
                User.LastName = model.LastName;
                User.Enabled = model.Enabled;
                User.UtypeId = model.Utype_Id;
                await _localStorageService.SetItem(_userKey, User);
            }
        }

        public async Task Delete(string id)
        {
            await _httpService.Delete($"/api/Users/{id}");

            // auto logout if the logged in user deleted their own record
            if (id == User.UserId.ToString())
                await Logout();
        }
    }
}