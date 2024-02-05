using MeetingsDomain.Contracts.Request;
using MeetingsDomain.Services;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace MeetingsAPI.Services
{
    public class EmailApiService : IEmailApiService
    {
        private IHttpClientFactory _clientFactory;
        private readonly string _apiUrl;

        public EmailApiService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _apiUrl = "http://localhost:8080/api/email";
        }

        public async void Send(SendEmailRequest email)
        {
            var uri = _apiUrl + "/sendmail";
            var client = _clientFactory.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(email), Encoding.UTF8, "application/json");

            await client.PostAsync(uri, content);
        }
    }
}
