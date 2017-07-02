using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamoDBProject.Model;
using Amazon.DynamoDBv2;

namespace DynamoDBProject.Service
{
    interface IService
    {
        List<AppSettingPoco> getData();
    }
}
