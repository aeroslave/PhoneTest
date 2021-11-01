using System;
using System.IO;
using System.Net;

namespace PhoneNumberApi.Authentication
{
    public class CreatioLogin
    {
        private readonly string _appUrl;
        private readonly string _authServiceUrl;
        private readonly string _userName;
        private readonly string _userPassword;
        private CookieContainer _authCookie;
        
        public CreatioLogin(string appUrl, string userName, string userPassword)
        {
            _appUrl = appUrl;
            _authServiceUrl = _appUrl + @"/ServiceModel/AuthService.svc/Login";
            _userName = userName;
            _userPassword = userPassword;
        }

        public bool TryLogin()
        {
            var authData = @"{
                    ""UserName"":""" + _userName + @""",
                    ""UserPassword"":""" + _userPassword + @"""
                }";

            try
            {
                var request = CreateRequest(_authServiceUrl, authData);
                _authCookie = new CookieContainer();
                request.CookieContainer = _authCookie;
                using var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return false;
                }

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var responseMessage = reader.ReadToEnd();
                    Console.WriteLine(responseMessage);
                    if (responseMessage.Contains("\"Code\":1"))
                    {
                        throw new UnauthorizedAccessException($"Unauthorized {_userName} for {_appUrl}");
                    }
                }

                var authName = ".ASPXAUTH";
                var authCookeValue = response.Cookies[authName]?.Value;
                _authCookie.Add(new Uri(_appUrl), new Cookie(authName, authCookeValue));
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public CookieContainer GetAuthContainer()
        {
            return _authCookie;
        }

        public void AddCsrfToken(HttpWebRequest request)
        {
            var cookie = request.CookieContainer.GetCookies(new Uri(_appUrl))["BPMCSRF"];
            if (cookie != null)
            {
                request.Headers.Add("BPMCSRF", cookie.Value);
            }
        }

        public static HttpWebRequest CreateRequest(string url, string requestData = null)
        {
            var request = (HttpWebRequest) WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "POST";
            request.KeepAlive = true;
            if (string.IsNullOrEmpty(requestData))
            {
                return request;
            }

            using var requestStream = request.GetRequestStream();
            using var writer = new StreamWriter(requestStream);
            writer.Write(requestData);

            return request;
        }
    }
}