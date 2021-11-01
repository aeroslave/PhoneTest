using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhoneNumberApi.Services;

namespace PhoneNumberApi.Controllers
{
    /// <summary>
    /// Контроллер получения данных о владельце номера.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PhoneDataController : ControllerBase
    {
        /// <summary>
        /// Сервис аутентификации.
        /// </summary>
        private readonly IAuthService _authService;

        /// <summary>
        /// Сервис получения данных о владельце номера.
        /// </summary>
        private readonly IPhoneService _phoneService;

        /// <summary>
        /// Контроллер получения данных о владельце номера.
        /// </summary>
        public PhoneDataController(IAuthService authService, IPhoneService phoneService)
        {
            _authService = authService;
            _phoneService = phoneService;
        }

        /// <summary>
        /// Залогиниться
        /// </summary>
        [Route("Login")]
        [HttpPost]
        public void Login()
        {
            _authService.TryLogin();
        }

        /// <summary>
        /// Получить данные о владельце номера.
        /// </summary>
        [Route("GetPhoneData")]
        [HttpPost]
        public async Task<string> GetPhoneData(string phoneNumber)
        {
            if (_authService.IsLogged)
            {
                return await _phoneService.GetPhoneData(phoneNumber, _authService.GetToken());
            }

            throw new UnauthorizedAccessException();
        }
    }
}