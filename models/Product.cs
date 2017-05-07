using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Training
{
    public class Product
    {   
        public string ProductID { get; set; }
        public string Name { get; set; }
        public string ProductNumber { get; set; }
        public string Color { get; set; }
        public decimal StandardCost { get; set; }
        public decimal ListPrice { get; set; }
        public string Size { get; set; }
        public decimal? Weight { get; set; }
        public DateTime? SellStartDate { get; set; }
        public DateTime? SellEndDate { get; set; }
        public DateTime? DiscontinuedDate { get; set; }
        public string CategoryName { get; set; }
        public string ModelName { get; set; }
        public string Description { get; set; }

        public static List<Product> Seed()
        {
            var data = File.ReadAllText(@"./models/seed.json");
            return JsonConvert.DeserializeObject<List<Product>>(data);
        }

        public static string Data() {
            return JsonConvert.SerializeObject(Product.Seed());
        }
    }	
}   
   
   
