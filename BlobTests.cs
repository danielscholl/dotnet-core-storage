using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Training
{
    [TestClass]
    public class BlobTests
    {
        readonly string containerName = "testcontainer";

        private CloudBlobContainer GetContainer()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Config.Data.GetConnectionString());
            CloudBlobClient client = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(containerName);
            return container;
        }

        [TestMethod]
        public async Task CreateContainer()
        {
            // Get the Container Reference
            CloudBlobContainer container = GetContainer();

            // Create the Container if it doesn't exist;
            bool created = await container.CreateIfNotExistsAsync();

            Assert.IsTrue(created, "Container should be created");
        }

        [TestMethod]
        public async Task GetPermissions()
        {
            // Get the Container Reference
            CloudBlobContainer container = GetContainer();

            // Fetch the Attributes to fill up the Container Properties
            await container.FetchAttributesAsync();

            Assert.AreEqual(container.Properties.PublicAccess, BlobContainerPublicAccessType.Off, "Default Blob Access Level is Private");
        }

        [TestMethod]
        public async Task SetPermissions()
        {
            // Get the Container Reference
            CloudBlobContainer container = GetContainer();

            // Setup the Permissions for Blob Access
            BlobContainerPermissions permissions = new BlobContainerPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
            await container.SetPermissionsAsync(permissions);

            // Fetch the Attributes to fill up the Container Properties
            await container.FetchAttributesAsync();

            Assert.AreEqual(container.Properties.PublicAccess, BlobContainerPublicAccessType.Blob, "Set Blob Access Level is Blob");
        }

        [TestMethod]
        public async Task UploadTest()
        {
            // Get the Container Reference
            CloudBlobContainer container = GetContainer();

            CloudBlockBlob blob = container.GetBlockBlobReference("seed.json");
            await blob.UploadTextAsync(Product.Data());


            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task ListBlobsTest()
        {
            // Get the Container Reference
            CloudBlobContainer container = GetContainer();

            // Get list of Blobs default amount of 5000 with no continue Token.
            BlobResultSegment page = await container.ListBlobsSegmentedAsync(null);

            // Pull the filenames and put them into a list
            var list = new List<string>();
            foreach (var item in page.Results)
            {
                string file = item.Uri.ToString();
                int index = file.IndexOf(containerName);
                string name = file.Substring(index + containerName.Length + 1, file.Length - (index + containerName.Length + 1));
                list.Add(name);
            }

            Assert.AreEqual(1, list.Count);
        }

        [TestMethod]
        public async Task DownloadTest() {

            // Get the Container Reference
            CloudBlobContainer container = GetContainer();

            // Get the Reference to the Blob
            CloudBlockBlob blob = container.GetBlockBlobReference("seed.json");

            if(await blob.ExistsAsync())
            {
                String data = await blob.DownloadTextAsync();
                Assert.IsNotNull(data);
            }
        }

        [TestMethod]
        public async Task CopyBlob()
        {
            // Get the Container Reference
            CloudBlobContainer container = GetContainer();

            // Get the Reference to the Blob
            CloudBlockBlob blob = container.GetBlockBlobReference("seed.json");
            CloudBlockBlob target = container.GetBlockBlobReference("seedcopy.json");

            // Copy the Blob
            string jobId = await target.StartCopyAsync(blob);
            Assert.IsNotNull(jobId, "Blob Copy Started");
        }

        [TestMethod]
        public async Task DeleteBlob()
        {
            // Get the Container Reference
            CloudBlobContainer container = GetContainer();

            // Get the Reference to the Blob
            CloudBlockBlob blob = container.GetBlockBlobReference("seedcopy.json");

            // Copy the Blob
            bool deleted = await blob.DeleteIfExistsAsync();
            Assert.IsTrue(deleted, "Blob deleted");
        }


        [TestMethod]
        public async Task DeleteContainer()
        {
            // Get the Container Reference
            CloudBlobContainer container = GetContainer();

            // Create the Container if it doesn't exist;
            bool created = await container.DeleteIfExistsAsync();

            Assert.IsTrue(created, "Container should be deleted");
        }
    }
}
