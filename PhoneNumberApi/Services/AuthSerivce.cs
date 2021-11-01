using System.Net;
using PhoneNumberApi.Authentication;
using PhoneNumberApi.Data;

namespace PhoneNumberApi.Services
{
    /// <summary>
    /// Сервис аутентификации.
    /// </summary>
    public class AuthSerivce : IAuthService
    {
        /// <summary>
        /// Инструмент аутенификации.
        /// </summary>
        private readonly CreatioLogin _creatioLogin;

        /// <summary>
        /// Сервис аутентификации.
        /// </summary>
        public AuthSerivce()
        {
            _creatioLogin = new CreatioLogin(Constants.Url, "interview", "AAaaa11!");
            IsLogged = false;
        }

        /// <summary>
        /// Залогинился ли пользователь.
        /// </summary>
        public bool IsLogged { get; set; }

        /// <summary>
        /// Попытаться залогиниться.
        /// </summary>
        public void TryLogin()
        {
            IsLogged = _creatioLogin.TryLogin();
        }

        /// <summary>
        /// Получить токен авторизации.
        /// </summary>
        /// <returns> </returns>
        public CookieContainer GetToken()
        {
            return _creatioLogin.GetAuthContainer();
        }
    }
}