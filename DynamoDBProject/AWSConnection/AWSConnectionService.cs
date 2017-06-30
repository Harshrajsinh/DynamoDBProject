/*created by Harshraj Thakor on 29 June 2017
 * 
 * This class provides functionality for connecting the Amazon DynamoDB
 * It create the credentials from the profile name
 * The credentials are used to create the AmazonDB Client
 * The amazon client is then used to create the Amazon DynamodB context.
 */

using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon;

namespace DynamoDBProject.AWSConnection
{
    public static class AWSConnectionService
    {
        //gretting the credentials 
        public static Amazon.Runtime.AWSCredentials crendentials = Amazon.Util.ProfileManager.GetAWSCredentials("cdkglobal");

        //getting the Amazon DB client
        //passing the credentials and the region as the parameters 
        public static AmazonDynamoDBClient client = new AmazonDynamoDBClient(crendentials, RegionEndpoint.USWest2);

        //creating the context of the DynamoDB by passing the DynamoDBClient
        public static DynamoDBContext context = new DynamoDBContext(client);
    }
}
