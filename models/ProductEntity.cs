using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Training.models
{
    class ProductEntity : TableEntity
    {
        public string Name { get; set; }
        public string ProductNumber { get; set; }
        public string Color { get; set; }
        public decimal StandardCost { get; set; }
        public decimal ListPrice { get; set; }
        public string Size { get; set; }
        //public decimal? Weight { get; set; }
        //public DateTime? SellStartDate { get; set; }
        //public DateTime? SellEndDate { get; set; }
        //public DateTime? DiscontinuedDate { get; set; }
        
        public string ModelName { get; set; }
        public string Description { get; set; }

        public ProductEntity(Product product)
        {
            this.PartitionKey = product.CategoryName;
            this.RowKey = product.ProductID;

            this.Name = product.Name;
            this.ProductNumber = product.ProductNumber;
            this.Color = product.Color;
            this.StandardCost = product.StandardCost;
            this.ListPrice = product.ListPrice;
            this.Size = product.Size;
            //this.Weight = product.Weight;
            //this.SellStartDate = product.SellStartDate;
            //this.SellEndDate = product.SellEndDate;
            //this.DiscontinuedDate = product.DiscontinuedDate;
            this.ModelName = product.ModelName;
            this.Description = product.Description;
        }

        public ProductEntity()
        {

        }
    }
}
