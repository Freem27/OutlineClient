using System;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace TDV.OutlineClient
{
    public interface IOutlineClient
    {
        IOutlineAccessKey AccessKeys { get; }
        IOutlineMetrics Metrics { get; }
        IOutlineServer Server { get; }
    }

    public class OutlineClient : IOutlineClient
    {
        private readonly string _apiUrl;
        private readonly string _certSha256;
        internal readonly HttpClient _httpClient;

        public OutlineClient(string apiUrl, string certSha256)
        {
            _apiUrl = apiUrl;
            _certSha256 = certSha256;

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
            {
                if (sslPolicyErrors == SslPolicyErrors.None)
                {
                    return true;
                }

                using (var hasher = SHA256.Create())
                {
                    var hash = hasher.ComputeHash(cert.RawData);
                    return _certSha256 == BitConverter.ToString(hash).Replace("-", "");
                }
            };
            _httpClient = new HttpClient(clientHandler);
        }

        public IOutlineServer Server => new OutlineServer(this);
        public IOutlineAccessKey AccessKeys => new OutlineAccessKey(this);
        public IOutlineMetrics Metrics => new OutlineMetrics(this);


        internal async Task<T> Get<T>(string url)
        {
            var resp = await _httpClient.GetAsync(_apiUrl + url);
            resp.EnsureSuccessStatusCode();
            var body = await resp.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new ApplicationException("unable to deserialize");
        }

        internal async Task<T> Post<T>(string url, object postData)
        {
            var req = new StringContent(
                JsonSerializer.Serialize(postData, JsonSerializerOptions)
                , Encoding.UTF8, Application.Json
            );
            var resp = await _httpClient.PostAsync(_apiUrl + url, req);
            resp.EnsureSuccessStatusCode();
            var body = await resp.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new ApplicationException("unable to deserialize");
        }

        internal async Task<T> Put<T>(string url, object postData)
        {
            var req = new StringContent(
                JsonSerializer.Serialize(postData, JsonSerializerOptions)
                , Encoding.UTF8, Application.Json
            );
            var resp = await _httpClient.PutAsync(_apiUrl + url, req);
            resp.EnsureSuccessStatusCode();
            var body = await resp.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new ApplicationException("unable to deserialize");
        }

        internal async Task<HttpResponseMessage> Put(string url, object postData)
        {
            var req = new StringContent(
                JsonSerializer.Serialize(postData, JsonSerializerOptions)
                , Encoding.UTF8, Application.Json
            );
            return await _httpClient.PutAsync(_apiUrl + url, req);
        }
        internal async Task<HttpResponseMessage> Delete(string url)
        {
            return await _httpClient.DeleteAsync(_apiUrl + url);
        }

        private JsonSerializerOptions JsonSerializerOptions
        {
            get
            {
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                return options;
            }
        }
    }
}
