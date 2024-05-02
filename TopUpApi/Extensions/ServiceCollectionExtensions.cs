using TopUpApi.Service;

namespace TopUpApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddTopUpApi(this WebApplicationBuilder builder)
        {
            var serviceCollection = builder.Services;
            serviceCollection.AddSingleton<ICyberSourceHttpClient>(sp =>
                {
                    var apiKey = "";
                    var secretKey = "";
                    return new CyberSourceHttpClient(apiKey, secretKey);
                })
                // Add other services as needed
                .BuildServiceProvider();
            serviceCollection.AddScoped<ITopUpService, TopUpService>();
        }
    }
}
