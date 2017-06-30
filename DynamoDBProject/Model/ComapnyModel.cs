using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Amazon.DynamoDBv2.DataModel;

namespace DynamoDBProject.Model
{
    [DynamoDBTable("Company")]
    class ComapnyModel
    {
        [DynamoDBProperty("CompanyID")]
        public int companyID { get; set; }

        [DynamoDBProperty("CompanyName")]
        public string companyName { get; set; }

        [DynamoDBProperty("ComapnyCustomers")]
        public long companyCustomers { get; set; }

        [DynamoDBProperty("CompanyValue")]
        public double companyValue { get; set; }
    }
}
