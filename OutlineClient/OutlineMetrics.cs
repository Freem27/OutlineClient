using System.Collections.Generic;
using System.Threading.Tasks;
using TDV.OutlineClient.Models;

namespace TDV.OutlineClient
{
    public interface IOutlineMetrics
    {
        Task<List<MetricsTransfer>> List();
    }

    public class OutlineMetrics : IOutlineMetrics
    {
        private readonly OutlineClient _client;

        public OutlineMetrics(OutlineClient client)
        {
            _client = client;
        }

        public async Task<List<MetricsTransfer>> List()
        {
            var resp = await _client.Get<MetricsTransferResponse>("metrics/transfer");
            var result = new List<MetricsTransfer>();
            foreach (var kvp in resp.BytesTransferredByUserId)
            {
                result.Add(new MetricsTransfer { AccessKeyId = kvp.Key, Bytes = kvp.Value });
            }

            return result;
        }
    }
}
