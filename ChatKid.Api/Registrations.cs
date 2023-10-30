using AutoMapper;
using ChatKid.Api.Services.Mapping;
using ChatKid.ApiFramework.AuthTokenIssuer;

namespace ChatKid.Api
{
    public static class Registrations
    {
        public static IServiceCollection RegisterApiService(this IServiceCollection services)
        {
            services.AddSingleton<ITokenIssuer, TokenIssuer>();

            services.AddSingleton<IMapper>(provider =>
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MappingProfile());
                });
                return mappingConfig.CreateMapper();
            });

            return services;
        }
    }
}
