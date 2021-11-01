using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PhoneNumberApi.Data;
using PhoneNumberApi.Models;

namespace PhoneNumberApi.Services
{
    /// <summary>
    /// Сервис для получения данных о владельце номера.
    /// </summary>
    public class PhoneService : IPhoneService
    {
        /// <summary>
        /// Http клиент.
        /// </summary>
        private readonly HttpClientHandler _httpClientHandler;

        /// <summary>
        /// Сервис для получения данных о владельце номера.
        /// </summary>
        public PhoneService()
        {
            _httpClientHandler = new HttpClientHandler {UseCookies = false};
        }

        /// <summary>
        /// Получить данные о владельце номера.
        /// </summary>
        public async Task<string> GetPhoneData(string mobile, CookieContainer authContainer)
        {
            var mobileNumber = new MobileNumber {Mobile = mobile};
            var uri = new Uri(Constants.Url);
            using var client = new HttpClient(_httpClientHandler){BaseAddress = uri };

            var jsonInString = JsonConvert.SerializeObject(mobileNumber);
            var message = new HttpRequestMessage(HttpMethod.Post, $"{Constants.Url}/0/rest/InfintoPortalService/GetClientInfo")
            {
                Content = new StringContent(jsonInString, Encoding.UTF8, "application/json")
            };

            var authCookie = authContainer.GetCookies(uri).FirstOrDefault(it => it.Name == ".ASPXAUTH");
            if (authCookie != null)
            {
                message.Headers.Add("Cookie", $"{authCookie.Name}={authCookie.Value}");
            }

            var response = await client.SendAsync(message);
            return await response.Content.ReadAsStringAsync();
        }
    }
}