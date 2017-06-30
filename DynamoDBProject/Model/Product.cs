using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDBProject.Model
{
    [DynamoDBTable("ProductTablle")]
    class Product
    {
        //partition key
        [DynamoDBHashKey("ProductId")]
        public int ProductId { get; set; }

        [DynamoDBProperty("ProductName")]
        public string ProductName { get; set; }

        [DynamoDBProperty("ProductPrice")]
        public int ProductPrice { get; set; }

        [DynamoDBProperty("{ProductOwner")]
        public string productOwner { get; set; }
    }
}
