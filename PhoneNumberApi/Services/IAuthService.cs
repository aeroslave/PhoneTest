using System.Net;

namespace PhoneNumberApi.Services
{
    public interface IAuthService
    {
        bool IsLogged { get; set; }

        void TryLogin();

        CookieContainer GetToken();
    }
}