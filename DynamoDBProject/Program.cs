//sample console application for testing DynamoDb
//contains various methods for creating, deleting tables
//contains two types of methods to query the table --> scan and query

using System;
using System.Collections.Generic;

using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

using DynamoDBProject.AWSConnection;
using DynamoDBProject.Model;
using Amazon.DynamoDBv2.Model;
using DynamoDBProject.CrudOperations;
using System.Threading.Tasks;
using System.Configuration;
using System.Threading;

namespace DynamoDBProject
{
    class Program
    {
        private static int _productId;
        public static void Main(string[] args)
        {
            try
            {
                createTableQueries();
                saveData();
                queryTable();
                scanTable();
                retrieveData();
                deleteTable();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message + "         " + ex.Source+"          "+ex.InnerException);
            }
            Console.Read();
        }

        //deleting the table
        public static void deleteTable()
        {
            //deleting the table
            Task<DeleteTableResponse> deleteTableResponse = CrudOperation.deleteTable(AWSConnectionService.client, "Company");
            if (deleteTableResponse.IsCompleted)
                Console.WriteLine("deleted");
            else
                Console.WriteLine("not deleted");

        }

        //queries for creating the table 
        public static void createTableQueries()
        { 
            //createTable(client,"Compnay");
            Task<CreateTableResponse> createTableResponse = CrudOperation.createTableAsync(AWSConnectionService.client, "Company");
            if (createTableResponse.IsCompleted)
                Console.WriteLine("created");
            else
                Console.WriteLine("not created");
        }

        //queries for the saving the data
        public static void saveData()
        {
            //adding the data from model class
            CrudOperation.saveProduct<ComapnyModel>(AWSConnectionService.context, AddData.companyModel);


            //creating the new instance of the product
            //and saving in the database 
            //_productId = 3;
            Product productDetails = new Product
            {
                ProductId = _productId,
                ProductName = "Phone",
                productOwner = "Cisco",
                ProductPrice = 500,
            };
            Task savedProductTask = AWSConnectionService.context.SaveAsync<Product>(productDetails);  //using the context (not the DBClient)
            if (savedProductTask.IsCompleted)
                Console.WriteLine("Completed");
            else
                Console.WriteLine("InComplete");
        }



        //querying the table
        public static void queryTable()
        {
            //calling the method which is in another class 
            Task<QueryResponse> taskQueryResponse = CrudOperation.queryTable(AWSConnectionService.client, "Company");
            QueryResponse queryResponse = taskQueryResponse.Result;
            List<Dictionary<string, AttributeValue>> queryResponseItems = queryResponse.Items;
            foreach (Dictionary<string, AttributeValue> keyValuePair in queryResponseItems)
            {
                CrudOperation.printItem(keyValuePair);
            }


            //getting the specific item and transforming the data into JSON data
            var table = Table.LoadTable(AWSConnectionService.client, "Company");
            var item = table.GetItem(1, "ABC Inc");
            var items = table.CreateBatchGet();
            var jItem = item.ToJson();
            Console.WriteLine(jItem);



            // creating the query request and passing the queryRequest to the QueryAsync
            QueryRequest queryRequest = new QueryRequest
            {
                TableName = "AppSettings",
                KeyConditionExpression = "KeyName = :keyName",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        {
                            ":keyName", new AttributeValue
                            {
                                S = "ASRPro"
                            }
                        }
                    }
            };
            Task<QueryResponse> taskQueryResponse1 = AWSConnectionService.client.QueryAsync(queryRequest);
            QueryResponse queryresponse = taskQueryResponse1.Result;
            List<Dictionary<string, AttributeValue>> queryResponseItems1 = queryresponse.Items;
            foreach (Dictionary<string, AttributeValue> kvp in queryResponseItems1)
            {
                CrudOperation.printItem(kvp);
            }
        }


        //scanning the table 
        public static void scanTable()
        {
            //scanning the table (Async Way)
            Task<ScanResponse> result = CrudOperation.scanTable(AWSConnectionService.client, "ProductTablle");
            ScanResponse response = result.Result;
            List<Dictionary<string, AttributeValue>> items1 = response.Items;
            foreach (Dictionary<string, AttributeValue> dic in items1)
            {
                CrudOperation.printItem(dic);
            }


            //scanning the table with help of the .Net Object Persistence Model
            IEnumerable<AppSettingPoco> queyResponse = AWSConnectionService.context.Scan<AppSettingPoco>(new ScanCondition("KeyName", ScanOperator.Equal, "ASRPro"));
            foreach (AppSettingPoco app in queyResponse)
            {
                Console.WriteLine(app.KeyName);
                Console.WriteLine("ASRProDemoSite: " + app.KeyValue.ASRProDemoSite);
            }
        }


        //retrieve data from the data 
        public static void retrieveData()
        {

            //another way of the retrieving the data from the table 
            /*making the request
            tableName, 
            ProjectExpression should contains the attributes names
            Key is expression (where expression)*/
            var request = new GetItemRequest
            {
                TableName = "ProductTablle",
                ProjectionExpression = "ProductId, ProductName",
                Key = new Dictionary<string, AttributeValue>
                    {
                        { "ProductId", new AttributeValue { N = "1" }}
                    },
            };

            //getting one item by passing the request
            var anotherResponse = AWSConnectionService.client.GetItem(request);

            Console.WriteLine(anotherResponse.ResponseMetadata.RequestId);

            //print the items which the request fetched
            CrudOperation.printItem(anotherResponse.Item);

            //to retrieve product
            //passing the context to the method to retreive the data
            //_productId = 2;
            Task<Product> productRetrieved = CrudOperation.crudOperations(AWSConnectionService.context, _productId);
            Product p = productRetrieved.Result;
            Console.WriteLine(p.ProductId + "----------" + p.ProductName);
        }
    }
}