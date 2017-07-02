using DynamoDBProject.Model;
using DynamoDBProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDBProject
{
    class Client
    {
        private static IService _iService;
        public List<AppSettingPoco> results;

        public Client(IService iService)
        {
            _iService = iService;
        }
        
        public void postData()
        {
            Console.WriteLine("Inside the Another Main Class ==> ");
            results = _iService.getData();
            foreach (AppSettingPoco appSettingPoco in results)
            {
                Console.WriteLine("ASRProDemoSite: "+appSettingPoco.KeyValue.ASRProDemoSite);
                Console.WriteLine("LogException: " + appSettingPoco.KeyValue.LogException);
                Console.WriteLine("LogExceptionDatabase: " + appSettingPoco.KeyValue.LogExceptionDatabase);
                Console.WriteLine("LogExceptionEventLog: " + appSettingPoco.KeyValue.LogExceptionEventLog);
            } 
        }
    }
}
