using TDV.OutlineClient.Models;

namespace TDV.OutlineClient.Test
{
    [Collection("Enviroment collection")]
    public class AccessKeysTests
    {
        private OutlineClient _client;

        public AccessKeysTests()
        {
            _client = Shared.GetClient();
        }

        [Fact]
        public async Task List()
        {
            var newKey =  await _client.AccessKeys.New();
            try
            { 
                var sut = await _client.AccessKeys.List();

                Assert.NotNull(sut);
                Assert.Contains(sut, s => s.Id == newKey.Id);
            }
            finally
            {
                await _client.AccessKeys.Delete(newKey.Id);
            }
        }

        [Fact]
        public async Task Get()
        {
            var newKey = await _client.AccessKeys.New();
            try
            {
                var sut = await _client.AccessKeys.Get(newKey.Id);

                Assert.NotNull(sut);
                Assert.Equal(newKey.Id, sut.Id);
            }
            finally
            {
                await _client.AccessKeys.Delete(newKey.Id);
            }
        }

        [Theory]
        [InlineData(["aes-192-gcm", null, null, null, null, null])]
        [InlineData(["aes-192-gcm", "dafsdfad", null, null, null, null])]
        [InlineData(["aes-192-gcm", "dafsdfad", "Name", null, null, null])]
        [InlineData(["aes-192-gcm", "dafsdfad", "Name", "Password", null, null])]
        [InlineData(["aes-192-gcm", "dafsdfad", "Name", "Password", 4324, null])]
        [InlineData(["aes-192-gcm", "dafsdfad", "Name", "Password", 4324, (long)12321321])]
        [InlineData(["aes-192-gcm", null, "Name", "Password", 4324, (long)12321321])]
        public async Task New(string method = "aes-192-gcm", string? id = null, string? name = null, string? password = null, int? port = null, long? limit = null)
        {
            // Act
            AccessKey sut = await _client.AccessKeys.New(method, id, name, password, port, limit);

            // Assert
            try
            {
                Assert.NotNull(sut);
                if (id != null)
                {
                    Assert.Equal(id, sut.Id);
                }
                if (port != null)
                {
                    Assert.Equal(port.Value, sut.Port);
                }
                if (name != null)
                {
                    Assert.Equal(name, sut.Name);
                }
                if (password != null)
                {
                    Assert.Equal(password, sut.Password);
                }
                Assert.Equal(method, sut.Method);
                if (limit != null)
                {
                    Assert.Equal(limit.Value, sut.DataLimit?.Bytes);
                }
            }
            finally
            {
                // Remove a key
                await _client.AccessKeys.Delete(sut.Id);
            }
        }

        [Fact]
        public async Task RemoveKey()
        {
            // Arrange
            var keyForDel = await _client.AccessKeys.New();

            // Act
            await _client.AccessKeys.Delete(keyForDel.Id);
            var sut = await _client.AccessKeys.List();

            // Assert
            Assert.DoesNotContain(sut, s => s.Id == keyForDel.Id);
        }


        [Fact]
        public async Task Rename()
        {
            // Arrange
            var testKey = await _client.AccessKeys.New();
            var newName = "test_rename";

            try
            {
                // Act
                await _client.AccessKeys.Rename(testKey.Id, newName);
                var sut = await _client.AccessKeys.Get(testKey.Id);

                // Assert
                Assert.Equal(newName, sut.Name);
            }
            finally
            {
                await _client.AccessKeys.Delete(testKey.Id);
            }
        }


        [Fact]
        public async Task SetDataLimit()
        {
            // Arrange
            var testKey = await _client.AccessKeys.New();
            try
            {
                // Act
                await _client.AccessKeys.SetDataLimit(testKey.Id, 123);
                var sut = await _client.AccessKeys.Get(testKey.Id);

                // Assert
                Assert.NotNull(sut.DataLimit?.Bytes);
                Assert.Equal(123, sut.DataLimit.Bytes);
            }
            finally
            {
                await _client.AccessKeys.Delete(testKey.Id);
            }
        }


        [Fact]
        public async Task DeleteDataLimit()
        {
            // Arrange
            var testKey = await _client.AccessKeys.New();
            await _client.AccessKeys.SetDataLimit(testKey.Id, 123);

            try
            {
                // Act
                await _client.AccessKeys.DeleteDataLimit(testKey.Id);
                var sut = await _client.AccessKeys.Get(testKey.Id);

                // Assert
                Assert.Equal(0, sut.DataLimit?.Bytes ?? 0);
            }
            finally
            {
                await _client.AccessKeys.Delete(testKey.Id);
            }
        }
    }
}