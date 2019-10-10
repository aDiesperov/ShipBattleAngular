using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ShipBattleAngularApi.BusinessLogic.Models;
using ShipBattleAngularApi.BusinessLogic.Services.Interfaces;
using ShipBattleApi.Models.Enums;

namespace ShipBattleAngularApi.BusinessLogic.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly string _clientController = "game";
        private readonly string _clientMethodJoin = "join";
        private readonly string _clientMethodLeft = "left";
        private readonly string _clientMethodStart = "start";
        private readonly string _clientMethodAddShip = "addShip";
        private readonly string _clientMethodReady = "ready";
        private readonly string _clientMethodMove = "move";
        private readonly string _clientMethodShot = "shot";
        private readonly string _clientMethodFix = "fix";
        private readonly string _clientMethodNextStep = "nextStep";

        private readonly AppSettings _opt;
        private readonly HttpClient _httpClient;
        private readonly string _mediaType = "application/json";

        public HttpClientService(IOptions<AppSettings> opt)
        {
            _opt = opt.Value;
            _httpClient = new HttpClient();
        }

        public async Task<string> AddShip(string user, string infoShip)
        {
            string url = createUrl(_opt.AddressApi, _clientController, _clientMethodAddShip, user);
            string content = JsonConvert.SerializeObject(infoShip);

            HttpContent httpContent = new StringContent(content, Encoding.UTF8, _mediaType);

            HttpResponseMessage httpResponse = await _httpClient.PostAsync(url, httpContent);
            if (httpResponse.IsSuccessStatusCode)
            {
                return await httpResponse.Content.ReadAsStringAsync();
            }
            return null;
        }

        public async Task<bool> Join(string user, string pathBase)
        {
            string url = createUrl(_opt.AddressApi, _clientController, _clientMethodJoin, user, pathBase);

            return await handlerResponseBool(await _httpClient.GetAsync(url));
        }

        public async Task Left(string user)
        {
            string url = createUrl(_opt.AddressApi, _clientController, _clientMethodLeft, user);

            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Head, url);
            await _httpClient.SendAsync(httpRequest);
        }

        public async Task<bool> Ready(string user)
        {
            string url = createUrl(_opt.AddressApi, _clientController, _clientMethodReady, user);

            return await handlerResponseBool(await _httpClient.GetAsync(url));
        }

        public async Task<bool> Start(string user, string enemy)
        {
            string url = createUrl(_opt.AddressApi, _clientController, _clientMethodStart, user, enemy);

            return await handlerResponseBool(await _httpClient.GetAsync(url));
        }

        private async Task<bool> handlerResponseBool(HttpResponseMessage httpResponse)
        {
            if (httpResponse.IsSuccessStatusCode)
            {
                string res = await httpResponse.Content.ReadAsStringAsync();
                if (Convert.ToBoolean(res))
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        private string createUrl(string url, string clientController, string clientMethod, params string[] prs)
        {
            Url ur = Url.Combine(url, clientController, clientMethod);
            ur.AppendPathSegments(prs);
            return ur.Path;
        }

        public async Task<bool> Move(string user, int num, int x, int y)
        {
            string url = createUrl(_opt.AddressApi, _clientController, _clientMethodMove, user);
            string content = JsonConvert.SerializeObject(new { num, x, y });

            HttpContent httpContent = new StringContent(content, Encoding.UTF8, _mediaType);
            return await handlerResponseBool(await _httpClient.PostAsync(url, httpContent));
        }

        public async Task<StateShot> Shot(string user, int num, int x, int y)
        {
            string url = createUrl(_opt.AddressApi, _clientController, _clientMethodShot, user);
            string content = JsonConvert.SerializeObject(new { num, x, y });

            HttpContent httpContent = new StringContent(content, Encoding.UTF8, _mediaType);
            HttpResponseMessage httpResponse = await _httpClient.PostAsync(url, httpContent);
            if (httpResponse.IsSuccessStatusCode)
            {
                var res = await httpResponse.Content.ReadAsStringAsync();
                return (StateShot)Convert.ToInt32(res);

            }
            return StateShot.Miss;
        }

        public async Task<bool> Fix(string user, int num, int x, int y)
        {
            string url = createUrl(_opt.AddressApi, _clientController, _clientMethodFix, user);
            string content = JsonConvert.SerializeObject(new { num, x, y });

            HttpContent httpContent = new StringContent(content, Encoding.UTF8, _mediaType);
            return await handlerResponseBool(await _httpClient.PostAsync(url, httpContent));
        }

        public async Task<bool> NextStep(string user)
        {
            string url = createUrl(_opt.AddressApi, _clientController, _clientMethodNextStep, user);

            return await handlerResponseBool(await _httpClient.GetAsync(url));
        }
    }
}
