using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Identity;
using ChatKid.DataLayer.Repositories;
using ChatKid.DataLayer.Repositories.Interfaces;
using ChatKid.DataLayer.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace ChatKid.DataLayer
{
    public static class Registrations
    {
        public static IServiceCollection RegisterDataLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDBContext, KidtalkieContext>();

            var dbConnectionStrings = configuration.GetSection("ConnectionStrings");
            services.AddDbContext<KidtalkieContext>(opts => opts.UseNpgsql(dbConnectionStrings["ChatKidDB"], dbo => dbo.EnableRetryOnFailure()));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<KidtalkieContext>()
                    .AddDefaultTokenProviders();

            services.RegisterRepository();
            return services;
        }
        public static IServiceCollection RegisterRepository(this IServiceCollection services)
        {
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IFamilyRepository, FamilyRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOtpRepository, OtpRepository>();
            services.AddScoped<IAdvertisingRepository, AdvertisingRepository>();
            services.AddScoped<INotificationRepository, Notificationrepository>();
            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<IExpertRepository, ExpertRepository>();
            services.AddScoped<IChannelRepository, ChannelRepository>();
            services.AddScoped<IChannelUserRepository, ChannelUserRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IKidServiceRepository, KidServiceRepository>();
            services.AddScoped<ISubcriptionRepository, SubcriptionRepository>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<ITypeBlogRepository, TypeBlogRepository>();
            services.AddScoped<IParentSubcriptionRepository, ParentSubcriptionRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IMoneyPaymentRepository, MoneyPaymentRepository>();
            services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IDiscussRoomRepository, DiscussRoomRepository>();
            return services;
        }
    }
}
