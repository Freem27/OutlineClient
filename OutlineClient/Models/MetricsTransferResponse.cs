using System.Collections.Generic;

namespace TDV.OutlineClient.Models
{
    internal class MetricsTransferResponse
    {
        public Dictionary<string, long> BytesTransferredByUserId { get; set; } = new Dictionary<string, long>();
    }
}
