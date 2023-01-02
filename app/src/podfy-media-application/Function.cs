using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;
using podfy_media_application.IoC;
using podfy_media_application.Service;
using podfy_media_application.Utils;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace podfy_media_application
{
    public class Function : HttpHelpers
    {
        public IMediaService mediaService { get; set; }

        public Function()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddServices();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            mediaService = serviceProvider.GetService<IMediaService>();
        }

        public async Task<APIGatewayProxyResponse> PutObjectFunctionHandlerAsync(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                context.Logger.LogInformation($"Novo arquivo para ser criado");
                var result = await mediaService.PutObjectAsync(request.Body);

                return result is null ? BadRequest("no file attachment or extension not allowed") : Ok(result);
            }
            catch (Exception ex)
            {
                context.Logger.LogError($"Aconteceu um error => {ex.Message}");
                return InternalServerError(ex.Message);
            }
        }

        public async Task<APIGatewayProxyResponse> GetObjectFunctionHandlerAsync(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                var queryParamValue = request.QueryStringParameters.Where(w => w.Key == "Key").FirstOrDefault().Value;

                context.Logger.LogInformation($"Obter arquivo por key: {queryParamValue}");

                var result = await mediaService.GetObjectByKeyAsync(queryParamValue);

                return result is null ? BadRequest("file dont exist") : Ok(result);
            }
            catch (Exception ex)
            {
                context.Logger.LogError($"Aconteceu um error => {ex.Message}");
                return InternalServerError(ex.Message);
            }
        }
    }
}