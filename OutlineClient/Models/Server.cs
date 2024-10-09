namespace TDV.OutlineClient.Models
{
    public class Server
    {
        public string Name { get; set; } = null!;
        public string ServerId { get; set; } = null!;
        public bool MetricsEnabled { get; set; }
        public long CreatedTimestampMs { get; set; }
        public string Version { get; set; } = null!;
        public int PortForNewAccessKeys { get; set; }
        public string HostnameForAccessKeys { get; set; } = null!;
        public Limit? AccessKeyDataLimit { get; set; }
    }
}
