using Amazon.S3;
using Amazon.S3.Model;
using podfy_media_application.Model;
using podfy_media_application.Services;
using System.Diagnostics;
using System.Text.Json;

namespace podfy_media_application.Service;

public class MediaService : IMediaService
{
    private readonly IS3BucketContext _s3BucketService;
    private readonly ISNSContext _sNSContext;
    public MediaService(IS3BucketContext s3BucketService, ISNSContext sNSContext)
    {
        _s3BucketService = s3BucketService;
        _sNSContext = sNSContext;
    }

    public async Task<ObjectResponse> PutObjectAsync(string base64encoded)
    {
        try
        {
            var bucketName = Debugger.IsAttached ? "your-bucket" : Environment.GetEnvironmentVariable("BUCKET_NAME");
            var snsArn = Debugger.IsAttached ? "your-bucket" : Environment.GetEnvironmentVariable("SNS_ARN");

            if (string.IsNullOrEmpty(base64encoded))
                return null;

            var baseSplited = base64encoded.Split(',');
            var contents = baseSplited[0].Replace("data:", "").Split(";");
            var contentType = contents[0];
            var extension = $".{contentType.Split('/')[1]}";
            string[] permittedExtensions = new string[] { ".mp3", ".wav", ".mpeg" };

            if (!permittedExtensions.Contains<string>(extension))
                return null;

            using (var stream = new MemoryStream(Convert.FromBase64String(baseSplited[1])))
            {
                stream.Position = 0;
                var identifier = Guid.NewGuid().ToString();
                var key = $"{identifier}{extension}";

                var request = new PutObjectRequest()
                {
                    InputStream = stream,
                    Key = key,
                    BucketName = bucketName,
                    ContentType = contentType,
                    CannedACL = S3CannedACL.PublicRead
                };

                var response = await _s3BucketService.PutObjectAsync(request);

                if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                    throw new ApplicationException("Media file upload to S3 failed");

                var fileUri = $"https://{bucketName}.s3.amazonaws.com/{key}";

                var message = new ObjectResponse {FileUri= fileUri, Identifier = identifier };

                await _sNSContext.PublishAsync(snsArn, JsonSerializer.Serialize(message));

                return message;
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<string> GetObjectByKeyAsync(string key)
    {
        try
        {
            var bucketName = Debugger.IsAttached ? "you-bucket" : Environment.GetEnvironmentVariable("BUCKET_NAME");

            var result = await _s3BucketService.GetFileByKeyAsync(bucketName, key);

            if (result is null)
                return null;

            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                result.ResponseStream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            return $"data:{result.Headers.ContentType};base64,{Convert.ToBase64String(bytes)}";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}

