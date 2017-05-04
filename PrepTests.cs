using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;

namespace Training
{
    [TestClass]
    public class PrepTests
    {
        [TestMethod]
        public void CanGetSecrets()
        {
            Assert.IsNotNull(Config.Data.Account);
            Assert.IsNotNull(Config.Data.Key);
            var connectionString = Config.Data.GetConnectionString();
            Assert.IsNotNull(connectionString, "Connection String should exist");
        }

        [TestMethod]
        public void CanGetProducts()
        {
            var products = Product.Seed();
            Assert.AreEqual(294, products.Count, "Product count should seed to 294");
        }


    }
}
