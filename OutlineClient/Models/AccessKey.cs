namespace TDV.OutlineClient.Models
{
    public class AccessKey
    {
        /// <summary>
        /// aes-192-gcm, chacha20-ietf-poly1305
        /// default: aes-192-gcm
        /// </summary>
        public string Method { get; set; } = "aes-192-gcm";
        public string Id { get; set; } = null!;
        public string AccessUrl { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = null!;
        public int Port { get; set; }
        public Limit? DataLimit { get; set; }
    }
}
