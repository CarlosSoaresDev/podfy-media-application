using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace podfy_media_application.Services
{
    public interface IS3BucketContext
    {
        Task<PutObjectResponse> PutObjectAsync(PutObjectRequest transferUtilityUploadRequest);

        Task UploadAsync(TransferUtilityUploadRequest transferUtilityUploadRequest);


         Task<GetObjectResponse> GetFileByKeyAsync(string bucketName, string key);

    }
}
