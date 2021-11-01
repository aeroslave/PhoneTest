using System.Net;
using System.Threading.Tasks;

namespace PhoneNumberApi.Services
{
    public interface IPhoneService
    {
        Task<string> GetPhoneData(string mobile, CookieContainer authContainer);
    }
}