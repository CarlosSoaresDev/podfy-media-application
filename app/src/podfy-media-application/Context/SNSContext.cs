using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System.Diagnostics;

namespace podfy_media_application.Services
{
    public class SNSContext : ISNSContext
    {


        public async Task<PublishResponse> PublishAsync(string topicArn, string messege)
        {
            try
            {
                return await GetSNSClient().PublishAsync(topicArn, messege);
            }
            catch (Exception ex)
            {
                throw;
            }           
        }

        private AmazonSimpleNotificationServiceClient GetSNSClient()
        {
            if (Debugger.IsAttached)
                return new AmazonSimpleNotificationServiceClient(Amazon.RegionEndpoint.USEast1);
            else
                return new AmazonSimpleNotificationServiceClient(Environment.GetEnvironmentVariable("ACCESS_KEY"), Environment.GetEnvironmentVariable("SECRET_KEY"), Amazon.RegionEndpoint.USEast1);
        }       
    }
}
