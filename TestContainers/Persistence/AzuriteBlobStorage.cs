using Azure.Storage;
using Azure.Storage.Blobs;
using System.Collections.Immutable;

namespace UsingTestContainers.Persistence
{
    public class AzuriteBlobStorage
    {
        private readonly Uri BlobUri;
        private readonly string AccountName;
        private readonly string AccountKey;
        private readonly BlobContainerClient Client;

        public AzuriteBlobStorage(int port)
        {
            this.BlobUri = new Uri($"http://127.0.0.1:{port}/devstoreaccount1/files");
            this.AccountName = "devstoreaccount1";
            this.AccountKey = "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==";


            this.Client = new BlobContainerClient(
                BlobUri,
                new StorageSharedKeyCredential(AccountName, AccountKey)
            );

            Client.CreateIfNotExists();
        }
        public ICollection<string> RetrieveProjectFiles()
        {
            var blobs = Client.GetBlobs();

            return blobs.Select(a => a.Name).ToImmutableList();
        }

        public bool StoreProjectFile(string name, byte[] data)
        {
            Client.UploadBlob(name, new BinaryData(data));

            return true;
        }
        public bool IsHealthy()
        {
            return Client.Exists().Value;
        }
    }
}
