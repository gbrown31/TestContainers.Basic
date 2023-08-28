using DotNet.Testcontainers.Containers;

namespace UsingTestContainers
{
    public class ContainerTest : IClassFixture<ContainerFixture>
    {
        private readonly ContainerFixture _fixture;
        public ContainerTest(ContainerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void DbContainer_IsHealthy()
        {
            Assert.Equal(TestcontainersStates.Running, _fixture.DbContainer.State);
        }

        [Fact]
        public void AzureContainer_IsHealthy()
        {
            Assert.Equal(TestcontainersStates.Running, _fixture.BlobContainer.State);
        }
    }
}