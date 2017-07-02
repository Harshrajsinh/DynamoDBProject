using DynamoDBProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamoDBProject.Model;
using DynamoDBProject.AWSConnection;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace DynamoDBProject
{
    class ServiceConnection : IService
    {
        public List<AppSettingPoco> getData()
        {
            ScanCondition scan1 = new ScanCondition("KeyName", ScanOperator.Equal, "ASRPro");
            List<ScanCondition> scanList = new List<ScanCondition>();
            scanList.Add(scan1);
            IEnumerable<ScanCondition> scanCondition = scanList;
            AsyncSearch<AppSettingPoco> result = AWSConnectionService.context.ScanAsync<AppSettingPoco>(scanList);
            Task<List<AppSettingPoco>> result1 = result.GetRemainingAsync();
            return result1.Result;
            //throw new NotImplementedException();
        }
    }
}
