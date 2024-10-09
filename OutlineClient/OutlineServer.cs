using System;
using System.Threading.Tasks;
using TDV.OutlineClient.Models;

namespace TDV.OutlineClient
{
    public interface IOutlineServer
    {
        Task DeleteDataLimit();
        Task<Server> Info();
        Task Rename(string name);
        Task SetDataLimit(long limitBytes);
        Task SetPortForNewAccessKeys(int port);
    }

    public class OutlineServer : IOutlineServer
    {
        private readonly OutlineClient _client;
        public OutlineServer(OutlineClient client)
        {
            _client = client;
        }

        public async Task<Server> Info()
        {
            return await _client.Get<Server>("server");
        }

        public async Task Rename(string name)
        {
            var resp = await _client.Put("name", new { name });
            if (resp.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                throw new ApplicationException("Server renaming error");
            }
        }

        public async Task SetPortForNewAccessKeys(int port)
        {
            var resp = await _client.Put("server/port-for-new-access-keys", new { port });
            resp.EnsureSuccessStatusCode();
        }

        public async Task SetDataLimit(long limitBytes)
        {
            var resp = await _client.Put("server/access-key-data-limit", new { limit = new { Bytes = limitBytes } });
            resp.EnsureSuccessStatusCode();
        }

        public async Task DeleteDataLimit()
        {
            var resp = await _client.Delete("server/access-key-data-limit");
            resp.EnsureSuccessStatusCode();
        }
        //TODO Changes the hostname for access keys
        //TODO Returns whether metrics is being shared
        //TODO Enables or disables sharing of metrics
    }
}
