using aemtest.Dto;
using aemtest.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aemtest.Services
{
    public class AemRestClient : IRestClient
    {
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl;
        private readonly string _authEndpoint;
        private readonly string _platformActualEndpoint;
        private readonly string _platformDummyEndpoint;
        private readonly string _userName;
        private readonly string _password;

        protected RestClient Client { get; set; }

        public AemRestClient(IConfiguration configuration)
        {
            _configuration = configuration;
            _baseUrl = _configuration["EnersolDemo:BaseUrl"];
            _authEndpoint = _configuration["EnersolDemo:AuthEndpoint"];
            _platformActualEndpoint = _configuration["EnersolDemo:PlatformActualEndpoint"];
            _platformDummyEndpoint = _configuration["EnersolDemo:PlatformDummyEndpoint"];
            _userName = _configuration["EnersolDemo:Username"];
            _password = _configuration["EnersolDemo:Password"];
            Client = new RestClient(_baseUrl);
        }

        public Task<string> Authenticate()
        {
            var request = new RestRequest(_authEndpoint, Method.POST);
            request.AddJsonBody(new AuthDto { Username = _userName, Password = _password });

            IRestResponse response = Client.Execute(request);

            return Task.FromResult(JsonConvert.DeserializeObject<string>(response.Content));
        }

        public async Task<IEnumerable<Platform>> GetPlatformWellActual()
        {
            return await GetPlatformWell(_platformActualEndpoint);
        }

        public async Task<IEnumerable<Platform>> GetPlatformWellDummy()
        {
            return await GetPlatformWell(_platformDummyEndpoint);
        }

        protected async Task<IEnumerable<Platform>> GetPlatformWell(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.GET);

            var token = await Authenticate();

            request.AddHeader("authorization", $"Bearer {token}");

            IRestResponse response = Client.Execute(request);

            return JsonConvert.DeserializeObject<IEnumerable<Platform>>(response.Content);
        }
    }
}
