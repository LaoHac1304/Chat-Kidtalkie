using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.Extensions.Configuration;
using ChatKid.GoogleServices.GoogleSettings;
using ChatKid.GoogleServices.GoogleAuthentication;
using ChatKid.GoogleServices.GoogleGmail;
using ChatKid.GoogleServices.GoogleCloudStorage;
using Google.Cloud.Storage.V1;
using Google.Apis.Auth.OAuth2;

namespace ChatKid.GoogleServices
{
    public static class Registrations
    {
        public static IServiceCollection RegisterGoogleService(this IServiceCollection services, IConfiguration configuration)
        {
            var _googleSettings = configuration.GetSection(GoogleConfigSettings.GoogleConfigSection).Get<GoogleConfigSettings>();
            services.AddSingleton(_ => _googleSettings);

            var _gmailSettings = configuration.GetSection(AppGmailSettings.AppGmailSettingsSection).Get<AppGmailSettings>();
            services.AddSingleton(_ => _gmailSettings);
            services.AddScoped<IGoogleAuthenticationService, GoogleAuthenticationService>();
            services.AddScoped<IGoogleGmailService, GoogleGmailService>();
            services.AddScoped<ICloudStorageService, CloudStorageService>();
            services.AddSingleton(_ =>
            {
                var googleCredential = GoogleCredential.FromFile(configuration.GetValue<string>("GoogleCredentialFile"));
                var storage =  StorageClient.Create(googleCredential);
                return storage;
            });
            return services;
        }
    }
}
