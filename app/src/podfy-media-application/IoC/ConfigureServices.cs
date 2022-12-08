using Microsoft.Extensions.DependencyInjection;
using podfy_media_application.Service;
using podfy_media_application.Services;

namespace podfy_media_application.IoC;

internal static class ConfigureServices
{
    public static void AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IS3BucketContext, S3BucketContext>();
        serviceCollection.AddTransient<ISNSContext, SNSContext>();
        serviceCollection.AddTransient<IMediaService, MediaService>();
    }
}

