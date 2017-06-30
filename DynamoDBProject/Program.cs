//sample console application for testing DynamoDb
//contains various methods for creating, deleting tables
//contains two types of methods to query the table --> scan and query

using System;
using System.Collections.Generic;

using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

using DynamoDBProject.AWSConnection;
using DynamoDBProject.Model;

namespace DynamoDBProject
{
    class Program
    {
        private static int _productId;
        public static void Main(string[] args)
        {
            try
            {
                //Task<CreateTableResponse> createTableRespone = CrudOperation.createTableAsync(client, "Company");
                //Console.WriteLine(createTableRespone.Result.TableDescription.TableStatus);

                //CrudOperation.saveProduct<ComapnyModel>(context,AddData.companyModel);

                //Task<QueryResponse> taskQueryResponse = CrudOperation.queryTable(client, "Company");
                //QueryResponse queryResponse = taskQueryResponse.Result;
                //List<Dictionary<string, AttributeValue>> queryResponseItems = queryResponse.Items;
                //foreach(Dictionary<string,AttributeValue> keyValuePair in queryResponseItems)
                //{
                //    CrudOperation.printItem(keyValuePair);
                //}

                //var table = Table.LoadTable(client, "Company");
                //var item = table.GetItem(1,"ABC Inc");
                //var items = table.CreateBatchGet();
                //var jItem = item.ToJson();
                //Console.WriteLine(jItem);

                //QueryRequest queryRequest = new QueryRequest
                //{
                //    TableName = "AppSettings",
                //    KeyConditionExpression = "KeyName = :keyName",
                //    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                //    {
                //        {
                //            ":keyName", new AttributeValue
                //            {
                //                S = "ASRPro"
                //            }
                //        }
                //    }
                //};

                //CancellationToken ca = new CancellationToken();

                //Task<QueryResponse> taskQueryResponse = client.QueryAsync(queryRequest);
                //QueryResponse queryresponse = taskQueryResponse.Result;
                //List<Dictionary<string,AttributeValue>> queryResponseItems = queryresponse.Items;
                //foreach(Dictionary<string,AttributeValue> kvp in queryResponseItems)
                //{
                //    CrudOperation.printItem(kvp);
                //}

                IEnumerable<AppSettingPoco> queyResponse = AWSConnectionService.context.Scan<AppSettingPoco>(new ScanCondition("KeyName",ScanOperator.Equal,"ASRPro"));
                foreach (AppSettingPoco app in queyResponse)
                {
                    Console.WriteLine(app.KeyName);
                    Console.WriteLine("ASRProDemoSite"+app.KeyValue.ASRProDemoSite);   
                }
                Console.Read();

                ////scanning the table
                //Task<ScanResponse> result = CrudOperation.scanTable(client, "ProductTablle");
                //ScanResponse response = result.Result;
                //List<Dictionary<string, AttributeValue>> items = response.Items;
                //foreach (Dictionary<string, AttributeValue> dic in items)
                //{
                //    CrudOperation.printItem(dic);
                //}



                ////quering the table
                //Task<QueryResponse> taskQueryResponse = CrudOperation.queryTable(client,ConfigurationManager.AppSettings["ProductTable"]);
                //QueryResponse queryresponse = taskQueryResponse.Result;
                //List<Dictionary<string, AttributeValue>> values1 = queryresponse.Items;
                //foreach (Dictionary<string,AttributeValue> dic in values1)
                //{
                //    CrudOperation.printItem(dic);
                //}


                ////creating the new instance of the product
                ////and saving in the database 
                ////_productId = 3;
                //Product saveProduct = new Product
                //{
                //    ProductId = _productId,
                //    ProductName = "Phone",
                //    productOwner = "Cisco",
                //    ProductPrice = 500,
                //};

                //Task savedProductTask = context.SaveAsync<Product>(saveProduct);  //using the context (not the DBClient)
                //if (savedProductTask.IsCompleted)
                //    Console.WriteLine("Completed");
                //else
                //    Console.WriteLine("InComplete");


                ////to retrieve product
                ////passing the context to the method to retreive the data
                ////_productId = 2;
                //Task<Product> productRetrieved = CrudOperation.crudOperations(context, _productId);
                //Product p = productRetrieved.Result;
                //Console.WriteLine(p.ProductId + "----------" + p.ProductName);

                ////createTable(client,"Customer");
                //Task<CreateTableResponse> createTableResponse = CrudOperation.createTableAsync(client, "Company");
                //if (createTableResponse.IsCompleted)
                //    Console.WriteLine("created");
                //else
                //    Console.WriteLine("not created");


                ////deleting the table
                //Task<DeleteTableResponse> deleteTableResponse = CrudOperation.deleteTable(client, "Company");
                //if (deleteTableResponse.IsCompleted)
                //    Console.WriteLine("deleted");
                //else
                //    Console.WriteLine("not deleted");


                ////another way of the retrieving the data from the table 
                ///*making the request
                //tableName, 
                //ProjectExpression should contains the attributes names
                //Key is expression (where expression)*/
                //var request = new GetItemRequest
                //{
                //    TableName = "ProductTablle",
                //    ProjectionExpression = "ProductId, ProductName",
                //    Key = new Dictionary<string, AttributeValue>
                //    {
                //        { "ProductId", new AttributeValue { N = "1" }}
                //    },
                //};

                ////getting one item by passing the request
                //var anotherResponse = client.GetItem(request);

                //Console.WriteLine(anotherResponse.ResponseMetadata.RequestId);

                ////print the items which the request fetched
                //CrudOperation.printItem(anotherResponse.Item);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message + "         " + ex.Source+"          "+ex.InnerException);
            }
            Console.Read();
        }
    }
}