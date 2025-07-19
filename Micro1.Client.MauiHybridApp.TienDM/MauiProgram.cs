using Microsoft.Extensions.Logging;
using Micro1.Client.MauiHybridApp.TienDM.Services;

namespace Micro1.Client.MauiHybridApp.TienDM
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            // Configure HttpClient to use Ocelot Gateway
            builder.Services.AddScoped(sp =>
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
                };
                return new HttpClient(handler)
                {
                    BaseAddress = new Uri("https://10.0.2.2:7214/")
                };
            });

            // Register AppointmentService
            builder.Services.AddScoped<AppointmentService>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
