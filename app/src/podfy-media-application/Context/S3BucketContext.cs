using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System.Diagnostics;

namespace podfy_media_application.Services
{
    public class S3BucketContext : IS3BucketContext
    {
        public async Task UploadAsync(TransferUtilityUploadRequest transferUtilityUploadRequest)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(GetS3Client());
                await fileTransferUtility.UploadAsync(transferUtilityUploadRequest);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<PutObjectResponse> PutObjectAsync(PutObjectRequest putObjectRequest)
        {
            try
            {
                 return await GetS3Client().PutObjectAsync(putObjectRequest);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<GetObjectResponse> GetFileByKeyAsync(string bucketName, string key)
        {
            try
            {
                return await GetS3Client().GetObjectAsync(bucketName, key);
            }
            catch (Exception ex)
            {
                throw;
            }           
        }

        private AmazonS3Client GetS3Client()
        {
            if (Debugger.IsAttached)
                return new AmazonS3Client(Amazon.RegionEndpoint.USEast1);
            else
                return new AmazonS3Client(Environment.GetEnvironmentVariable("ACCESS_KEY"), Environment.GetEnvironmentVariable("SECRET_KEY"), Amazon.RegionEndpoint.USEast1);
        }       
    }
}
