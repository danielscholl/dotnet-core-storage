using Microsoft.VisualStudio.TestTools.UnitTesting;
using dotnet_core_storage;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace dotnet_core_storage
{
    [TestClass]
    public class KeyStore
    {
        [TestMethod]
        public void CanGetSecrets()
        {
            Assert.IsNotNull(Config.Secrets.Account);
            Assert.IsNotNull(Config.Secrets.Key);
        }

        [TestMethod]
        public void PrintsFirstTest()
        {
            var connectionString = $"DefaultEndpointsProtocol=https;AccountName={Config.Secrets.Account};AccountKey={Config.Secrets.Key}";
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            var client = storageAccount.CreateCloudTableClient();
            CloudTable table = client.GetTableReference("mytable");
            table.CreateIfNotExistsAsync();


            System.Console.WriteLine(connectionString);
            Assert.IsTrue(true);
        }
    }
}
