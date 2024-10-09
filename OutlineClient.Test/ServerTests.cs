namespace TDV.OutlineClient.Test
{
    [Collection("Enviroment collection")]
    public class ServerTests
    {
        private OutlineClient _client;

        public ServerTests()
        {
            _client = Shared.GetClient();
        }

        [Fact]
        public async Task GetInfo()
        {
            var sut = await _client.Server.Info();
            Assert.NotNull(sut);
            Assert.False(string.IsNullOrWhiteSpace(sut.Name));
            Assert.False(string.IsNullOrWhiteSpace(sut.ServerId));
            Assert.False(string.IsNullOrWhiteSpace(sut.Version));
            Assert.False(string.IsNullOrWhiteSpace(sut.HostnameForAccessKeys));
            Assert.True(sut.CreatedTimestampMs > 0);
            Assert.True(sut.PortForNewAccessKeys > 0);
        }

        [Fact]
        public async Task Rename()
        {
            // Arrange
            var info = await _client.Server.Info();
            string newName = info.Name + "_test";

            // Act
            await _client.Server.Rename(newName);
            var sut = await _client.Server.Info();

            // Assert
            Assert.Equal(newName, sut.Name);

            await _client.Server.Rename(info.Name);
        }

        [Fact]
        public async Task SetPortForNewAccessKeys()
        {
            // Arrange
            var oldPort = (await _client.Server.Info()).PortForNewAccessKeys;
            int newPort = 12333;

            // Act
            await _client.Server.SetPortForNewAccessKeys(newPort);
            var sut = await _client.Server.Info();

            // Assert
            Assert.Equal(newPort, sut.PortForNewAccessKeys);

            await _client.Server.SetPortForNewAccessKeys(oldPort);
        }

        [Fact]
        public async Task SetAndDeleteDataLimit()
        {
            // Arrange
            var oldLimit = (await _client.Server.Info()).AccessKeyDataLimit?.Bytes ?? 0;
            long newLimit = 555555555555;

            // Act
            await _client.Server.SetDataLimit(newLimit);
            var sut = await _client.Server.Info();

            // Assert
            Assert.Equal(newLimit, sut.AccessKeyDataLimit?.Bytes);

            // Act
            await _client.Server.DeleteDataLimit();
            sut = await _client.Server.Info();

            // Assert
            Assert.Equal(0, sut.AccessKeyDataLimit?.Bytes ?? 0);

            if (oldLimit > 0)
            {
                await _client.Server.SetDataLimit(oldLimit);
            }
            else
            {
                await _client.Server.DeleteDataLimit();
            }
        }
    }
}