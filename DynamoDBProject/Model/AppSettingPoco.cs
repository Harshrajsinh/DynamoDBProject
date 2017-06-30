using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;

namespace DynamoDBProject.Model
{
    [DynamoDBTable("AppSettings")]
    class AppSettingPoco
    {
        [DynamoDBHashKey("KeyName")]
        public string KeyName { get; set; }

        [DynamoDBProperty("KeyValue")]
        public ASRProClass KeyValue { get; set; }
    }
}
