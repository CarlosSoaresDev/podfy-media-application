using podfy_media_application.Model;

namespace podfy_media_application.Service;

public interface IMediaService
{
    Task<ObjectResponse> PutObjectAsync(string base64encoded);

    Task<string> GetObjectByKeyAsync(string key);
}
