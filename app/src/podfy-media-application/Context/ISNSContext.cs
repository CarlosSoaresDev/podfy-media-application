using Amazon.SimpleNotificationService.Model;

namespace podfy_media_application.Services
{
    public interface ISNSContext
    {

        Task<PublishResponse> PublishAsync(string bucketName, string key);

    }
}
