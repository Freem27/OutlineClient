namespace TDV.OutlineClient.Test
{
    [Collection("Enviroment collection")]
    public class MetricsTests
    {
        private OutlineClient _client;

        public MetricsTests()
        {
            _client = Shared.GetClient();
        }

        [Fact]
        public async Task Transfer()
        {
            var sut = await _client.Metrics.List();

            Assert.NotNull(sut);
        }
    }
}