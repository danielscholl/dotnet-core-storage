using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Training.models;

namespace Training
{
    [TestClass]
    public class TableTests
    {
        readonly string tableName = "TestTable";

        private CloudTable GetTable()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Config.Data.GetConnectionString());
            CloudTableClient client = storageAccount.CreateCloudTableClient();
            CloudTable table = client.GetTableReference(tableName);
            return table;
        }


        [TestMethod]
        public async Task CreateTable()
        {
            // Get the Table Reference
            CloudTable table = GetTable();

            // Create the Table if it doesn't exist;
            bool created = await table.CreateIfNotExistsAsync();

            Assert.IsTrue(created, "Table should be created");
        }


        [TestMethod]
        public async Task CreateItem()
        {
            // Create A Table Entity.
            List<Product> productList = Product.Seed();
            ProductEntity product = new ProductEntity(productList[0]);

            // Get the Table Reference
            CloudTable table = GetTable();

            // Create the TableOperation that inserts the table entity.
            TableOperation insertOperation = TableOperation.InsertOrReplace(product);

            // Execute the insert operation.
            TableResult result = await table.ExecuteAsync(insertOperation);
            Assert.AreEqual(204, result.HttpStatusCode, "HTTP Status Code 204 returned");
        }

        [TestMethod]
        public async Task ReadItem()
        {
            Product product = Product.Seed()[0];

            // Get the Table Reference
            CloudTable table = GetTable();

            // Create a retrieve operation that takes a product entity.
            var partitionKey = product.CategoryName;
            var rowKey = product.ProductID;
            TableOperation retrieveOperation = TableOperation.Retrieve<ProductEntity>(partitionKey, rowKey);

            // Execute the retrieve operation.
            TableResult result = await table.ExecuteAsync(retrieveOperation);
            Assert.AreEqual(200, result.HttpStatusCode, "HTTP Status Code 200 returned");
        }



        [TestMethod]
        public async Task BatchInsert()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Config.Data.GetConnectionString());
            CloudTableClient client = storageAccount.CreateCloudTableClient();
            CloudTable table = client.GetTableReference(tableName);

            var productList = Product.Seed().GroupBy(p => p.CategoryName)
                                                   .Select(grp => new { Partition = grp.Key, ProductList = grp.ToList() })
                                                   .ToList();

            foreach(var partition in productList)
            {
                TableBatchOperation batchOperation = new TableBatchOperation();
                partition.ProductList.ForEach(product => batchOperation.InsertOrReplace(new ProductEntity(product)));
                IList<TableResult> resultList = await table.ExecuteBatchAsync(batchOperation);

                foreach (TableResult result in resultList)
                {
                    Assert.AreEqual(204, result.HttpStatusCode, "HTTP Status Code 204 returned");
                }
            }
        }

        [TestMethod]
        public async Task RetrieveForPartition()
        {
            Product product = Product.Seed()[0];

            // Get the Table Reference
            CloudTable table = GetTable();

            // Construct the query operation for all product entities for a partition key.
            var partitionKey = product.CategoryName;
            TableQuery<ProductEntity> query = new TableQuery<ProductEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));

            // Execute the retrieve operation.
            TableQuerySegment<ProductEntity> resultSegment = await table.ExecuteQuerySegmentedAsync(query, null);
            Assert.IsTrue(resultSegment.Results.Count > 0);
        }

        [TestMethod]
        public async Task DeleteEntity()
        {
            Product product = Product.Seed()[0];

            // Get the Table Reference
            CloudTable table = GetTable();

            // Create a retrieve operation that takes a product entity.
            var partitionKey = product.CategoryName;
            var rowKey = product.ProductID;
            TableOperation retrieveOperation = TableOperation.Retrieve<ProductEntity>(partitionKey, rowKey);

            // Execute the retrieve operation.
            TableResult retrieve = await table.ExecuteAsync(retrieveOperation);
            ProductEntity deleteEntity = (ProductEntity)retrieve.Result;

            TableOperation deleteOperation = TableOperation.Delete(deleteEntity);

            // Execute the operation.
            TableResult deleteResult = await table.ExecuteAsync(deleteOperation);

            Assert.AreEqual(204, deleteResult.HttpStatusCode, "HTTP Status Code 204 returned");
        }

        [TestMethod]
        public async Task DeleteTable()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Config.Data.GetConnectionString());
            CloudTableClient client = storageAccount.CreateCloudTableClient();
            CloudTable table = client.GetTableReference(tableName);

            bool deleted = await table.DeleteIfExistsAsync();

            Assert.IsTrue(deleted, "Table should be deleted");
        }
    }
}
