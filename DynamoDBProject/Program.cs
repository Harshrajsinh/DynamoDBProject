using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
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
                //gretting the credentials 
                Amazon.Runtime.AWSCredentials cred = Amazon.Util.ProfileManager.GetAWSCredentials("cdkglobal");

                //getting the Amazon DB client
                //passing the credentials and the region as the parameters 
                AmazonDynamoDBClient client = new AmazonDynamoDBClient(cred, RegionEndpoint.USWest2);

                //creating the context of the DynamoDB by passing the DynamoDBClient
                DynamoDBContext context = new DynamoDBContext(client);

                //Task<ScanResponse> result = scanTable(context,"ProductTablle", client);
                //ScanResponse response = result.Result;
                //List<Dictionary<string, AttributeValue>> items = response.Items;
                //foreach (Dictionary<string,AttributeValue> dic in items)
                //{
                //    printitem(dic);
                //}


                Task<QueryResponse> taskQueryResponse = queryTable(client,"ProductTablle");
                QueryResponse queryresponse = taskQueryResponse.Result;
                List<Dictionary<string, AttributeValue>> values1 = queryresponse.Items;
                foreach (Dictionary<string,AttributeValue> dic in values1)
                {
                    printitem(dic);
                }


                //var request = new QueryRequest
                //{

                //};
                //Task<QueryResponse> queryresponse = client.QueryAsync(request);
                //IEnumerable<Product> products = (IEnumerable<Product>) queryresponse.Result;
                //foreach (Product p in products)
                //{
                //    Console.WriteLine(p.ProductId);
                //}



                //creating the new instance of the product
                //and saving in the database 
                //_productId = 3;
                //Product saveProduct = new Product
                //{
                //    ProductId = _productId,
                //    ProductName = "Phone",
                //    productOwner = "Cisco",
                //    ProductPrice = 500,
                //};

                //Task savedProductTask = context.SaveAsync<Product>(saveProduct);
                //if (savedProductTask.IsCompleted)
                //    Console.WriteLine("Completed");
                //else
                //    Console.WriteLine("InComplete");


                //to retrieve product
                //passing the context to the method to retreive the data
                //_productId = 2;
                //Task<Product> productRetrieved = crudOperations(context,_productId);
                //Product p = productRetrieved.Result;
                //Console.WriteLine(p.ProductId+"----------"+p.ProductName);

                //createTable(client,"Customer");

                //Task<CreateTableResponse> createTableResponse = createTableAsync(client, "Company");
                //if (createTableResponse.IsCompleted)
                //    Console.WriteLine("created");
                //else
                //    Console.WriteLine("not created");

                //Task<DeleteTableResponse> deleteTableResponse = deleteTable(client,"Company");
                //if (deleteTableResponse.IsCompleted)
                //    Console.WriteLine("deleted");
                //else
                //    Console.WriteLine("not deleted");


                //another way of the retrieving the data from the table 
                /*making the request
                tableName, 
                ProjectExpression should contains the attributes names
                Key is expression (where expression)*/

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
                //var response = client.GetItem(request);

                //Console.WriteLine(response.ResponseMetadata.RequestId);

                ////print the items which the request fetched
                //printItem(response.Item);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message + "         " + ex.Source+"          "+ex.InnerException);
            }
            Console.Read();
        }

        public static void printitem(Dictionary<string, AttributeValue> attr)
        {
            foreach (KeyValuePair<string, AttributeValue> keyvaluepair in attr)
            {
                if (keyvaluepair.Key.Equals("ProductId"))
                {
                    Console.WriteLine("---------------------------");
                    Console.Write(keyvaluepair.Key + " ");
                    getattributevalue(keyvaluepair.Value);
                }
                else
                    Console.Write(keyvaluepair.Key + " ");
                    getattributevalue(keyvaluepair.Value);
            }
        }

        public static void getattributevalue(AttributeValue value)
        {
            if (value.S != null)
            {
                Console.WriteLine(value.S);
            }
            else if (value.N != null)
            {
                Console.WriteLine(value.N);
            }
        }


        //method to do crudopeartions on the DynamoDB
        public async static Task<Product> crudOperations(DynamoDBContext context, int productId)
        {
            return await context.LoadAsync<Product>(productId);
        }


        //to save the product
        //to update the product
        public async void saveProduct(DynamoDBContext context, Product product)
        {
            await context.SaveAsync<Product>(product);
        }


        //simple create table request
        public static void createTable(AmazonDynamoDBClient client, string tableName)
        {
            var request = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition>()
                {
                    new AttributeDefinition
                    {
                        AttributeName = "CustomerId",
                        AttributeType = "N"
                    }
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "CustomerId",
                        KeyType = "Hash"
                    }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 1,
                    WriteCapacityUnits = 1,
                }
            };
            var response = client.CreateTable(request);
            Console.WriteLine(response.TableDescription.TableStatus);
        }

        //create table as async method
        public static async Task<CreateTableResponse> createTableAsync(AmazonDynamoDBClient client, string tableName)
        {
            List<AttributeDefinition> attributeDefinitions = new List<AttributeDefinition>
            {
                new AttributeDefinition
                {
                    AttributeName = "CompanyId",
                    AttributeType = "N"
                }
            };

            List<KeySchemaElement> keyelementSchemaList = new List<KeySchemaElement>
            {
                new KeySchemaElement
                {
                    AttributeName = "CompanyId",
                    KeyType = "HASH"
                }
            };

            ProvisionedThroughput provisionedThroughput = new ProvisionedThroughput
            {
                ReadCapacityUnits = 5,
                WriteCapacityUnits = 5
            };
            CancellationToken cancellationToken = new CancellationToken();
            return await client.CreateTableAsync(tableName, keyelementSchemaList, attributeDefinitions, provisionedThroughput, cancellationToken);
        }

        //deleteTable 
        public async static Task<DeleteTableResponse> deleteTable(AmazonDynamoDBClient client,string tableName)
        {
            return await client.DeleteTableAsync(tableName);
        }

        //scan the table
        public async static Task<ScanResponse> scanTable(DynamoDBContext context,string tableName,AmazonDynamoDBClient client){
            //return await context.ScanAsync<Product>( new ScanCondition("ProductName",ScanOperator.Equal,"Book"));
            ScanRequest request = new ScanRequest
            {
                TableName = tableName,
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {
                        ":val", new AttributeValue
                        {
                            N = "500"
                        }
                    },
                    {
                        ":val2", new AttributeValue
                        {
                            N ="700"
                        }
                    }
                },
                FilterExpression = "ProductPrice >= :val AND ProductPrice < :val2",
            };
            

            request.ProjectionExpression= "ProductId,ProductName,ProductPrice,ProductOwner";
            CancellationToken ca = new CancellationToken();
            return await client.ScanAsync(request,ca);
        } 

        //query the table
        public static async Task<QueryResponse> queryTable(AmazonDynamoDBClient client, string tableName)
        {
            QueryRequest queryRequest = new QueryRequest
            {
                TableName = tableName,
                KeyConditionExpression = "ProductId = :Id",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {
                        ":Id", new AttributeValue
                        {
                            N = "2"
                        }
                    }
                }
            };
            queryRequest.ProjectionExpression = "ProductId,ProductName,ProductPrice,ProductOwner";
            CancellationToken ca = new CancellationToken();
            return await client.QueryAsync(queryRequest,ca);
        }
    }
}