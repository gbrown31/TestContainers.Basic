using DotNet.Testcontainers.Builders;
using Testcontainers.Azurite;
using Testcontainers.SqlEdge;
using UsingTestContainers.Persistence;

namespace UsingTestContainers
{
    public class ContainerFixture : IAsyncLifetime
    {        
        private readonly CancellationTokenSource _cts = new(TimeSpan.FromMinutes(5));
        public const int DbPort = 1433;
        public const int BlobPort = 10000;
        private const string _saPassword = "example_123";

        public readonly SqlEdgeContainer DbContainer;
        public SqlEdgeDatabase DbContext { get; private set; }

        public readonly AzuriteContainer BlobContainer;
        public AzuriteBlobStorage FileStorage { get; private set; }

        public ContainerFixture()
        {
            DbContainer = new SqlEdgeBuilder()
                .WithImage("mcr.microsoft.com/azure-sql-edge:1.0.7")
                .WithExposedPort(DbPort)
                .WithEnvironment("ACCEPT_EULA", "Y")
                .WithEnvironment("SA_PASSWORD", _saPassword)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(DbPort))
                .Build();


            BlobContainer = new AzuriteBuilder()
                .WithImage("mcr.microsoft.com/azure-storage/azurite")
                .WithExposedPort(BlobPort)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(BlobPort))
                .Build();
        }

        public async Task InitializeAsync()
        {
            await DbContainer.StartAsync(_cts.Token).ConfigureAwait(false);
            await BlobContainer.StartAsync(_cts.Token).ConfigureAwait(false);

            int port = DbContainer.GetMappedPublicPort(DbPort);
            DbContext = new SqlEdgeDatabase(port, _saPassword);
            DbContext.Database.EnsureCreated();

            int storagePort = BlobContainer.GetMappedPublicPort(BlobPort);
            FileStorage = new AzuriteBlobStorage(storagePort);
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _cts.Dispose();
        }
    }
}
