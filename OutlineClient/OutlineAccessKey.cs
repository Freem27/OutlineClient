using System.Collections.Generic;
using System.Threading.Tasks;
using TDV.OutlineClient.Models;

namespace TDV.OutlineClient
{
    public interface IOutlineAccessKey
    {
        Task Delete(string keyId);
        Task DeleteDataLimit(string keyId);
        Task<AccessKey> Get(string id);
        Task<List<AccessKey>> List();
        Task<AccessKey> New(string method = "aes-192-gcm", string? id = null, string? name = null, string? password = null, int? port = null, long? limit = null);
        Task Rename(string keyId, string name);
        Task SetDataLimit(string keyId, long limitBytes);
    }

    public class OutlineAccessKey : IOutlineAccessKey
    {
        private readonly OutlineClient _client;

        public OutlineAccessKey(OutlineClient client)
        {
            _client = client;
        }

        public async Task<List<AccessKey>> List()
        {
            var resp = await _client.Get<AccessKeysResponse>("access-keys");
            return resp.AccessKeys;
        }

        public async Task<AccessKey> Get(string id)
        {
            var resp = await _client.Get<AccessKey>($"access-keys/{id}");
            return resp;
        }

        public async Task<AccessKey> New(string method = "aes-192-gcm", string? id = null, string? name = null, string? password = null, int? port = null, long? limit = null)
        {
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "method", method }
            };
            if (name != null)
            {
                data.Add("name", name);
            }
            if (password != null)
            {
                data.Add("password", password);
            }
            if (port != null)
            {
                data.Add("port", port);
            }
            if (limit != null)
            {
                data.Add("limit", new { bytes = limit });
            }

            AccessKey resp;
            if (id == null)
            {
                resp = await _client.Post<AccessKey>("access-keys", data);
            }
            else
            {
                resp = await _client.Put<AccessKey>($"access-keys/{id}", data);
            }
            return resp;
        }

        public async Task Delete(string keyId)
        {
            var resp = await _client.Delete($"access-keys/{keyId}");
            resp.EnsureSuccessStatusCode();
        }

        public async Task Rename(string keyId, string name)
        {
            var resp = await _client.Put($"access-keys/{keyId}/name", new { name });
            resp.EnsureSuccessStatusCode();
        }

        public async Task SetDataLimit(string keyId, long limitBytes)
        {
            var resp = await _client.Put($"access-keys/{keyId}/data-limit", new { limit = new { Bytes = limitBytes } });
            resp.EnsureSuccessStatusCode();
        }

        public async Task DeleteDataLimit(string keyId)
        {
            var resp = await _client.Delete($"access-keys/{keyId}/data-limit");
            resp.EnsureSuccessStatusCode();
        }
    }
}
