using System;
using System.Collections.Generic;
using System.Threading.Tasks;


//amazon libraries 
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DataModel;
using System.Threading;

namespace DynamoDBProject.CrudOperations
{
    class CrudOperation
    {

        public static void printItem(Dictionary<string, AttributeValue> attr)
        {
            foreach (KeyValuePair<string, AttributeValue> keyvaluepair in attr)
            {
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
            else if(value.M.Count > 0)
            {
                printItem(value.M);
            }
        }


        //method to do crudopeartions on the DynamoDB
        public async static Task<Product> crudOperations(DynamoDBContext context, int productId)
        {
            return await context.LoadAsync<Product>(productId);
        }


        //to save the product
        //to update the product
        public static async void saveProduct<T>(DynamoDBContext context, T customObject)
        {
            try
            {
                await context.SaveAsync<T>(customObject);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message+"         "+ex.Source);
            }
            
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

        //create AsyncTable method
        //tableName+Id is partition Key
        //tableName+NAme is sort key
        public static async Task<CreateTableResponse> createTableAsync(AmazonDynamoDBClient client, string tableName)
        {
            List<AttributeDefinition> attributeDefinitions = new List<AttributeDefinition>
            {
                new AttributeDefinition
                {
                    AttributeName = tableName+"ID",
                    AttributeType = "N"
                },

                new AttributeDefinition
                {
                    AttributeName = tableName+"Name",
                    AttributeType = "S"
                }
            };

            List<KeySchemaElement> keyelementSchemaList = new List<KeySchemaElement>
            {
                new KeySchemaElement
                {
                    AttributeName = tableName+"ID",
                    KeyType = "HASH"
                },

                new KeySchemaElement
                {
                    AttributeName = tableName+"Name",
                    KeyType = "RANGE"
                }
            };

            ProvisionedThroughput provisionedThroughput = new ProvisionedThroughput
            {
                ReadCapacityUnits = 2,
                WriteCapacityUnits = 2
            };
            CancellationToken cancellationToken = new CancellationToken();
            return await client.CreateTableAsync(tableName, keyelementSchemaList, attributeDefinitions, provisionedThroughput, cancellationToken);
        }

        //deleteTable 
        public async static Task<DeleteTableResponse> deleteTable(AmazonDynamoDBClient client, string tableName)
        {
            return await client.DeleteTableAsync(tableName);
        }

        //scan the table
        public async static Task<ScanResponse> scanTable(AmazonDynamoDBClient client, string tableName)
        {
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


            request.ProjectionExpression = "ProductId,ProductName,ProductPrice,ProductOwner";
            CancellationToken ca = new CancellationToken();
            return await client.ScanAsync(request, ca);
        }

        //query the table
        public static async Task<QueryResponse> queryTable(AmazonDynamoDBClient client, string tableName)
        {
            QueryRequest queryRequest = new QueryRequest
            {
                TableName = tableName,
                KeyConditionExpression = tableName + "id = :id",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {
                        ":Id", new AttributeValue
                        {
                            N = "1"
                        }
                    },
                    {
                        ":customers", new AttributeValue
                        {
                            N = "1000"
                        }
                    }
                }, 
                FilterExpression = "ComapnyCustomers = :customers",
            };
            //queryRequest.ProjectionExpression = "CompanyID,CompanyName,ComapnyCustomers,CompanyValue";
            CancellationToken ca = new CancellationToken();
            return await client.QueryAsync(queryRequest, ca);
        }
    }
}
