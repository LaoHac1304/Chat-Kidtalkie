using Microsoft.Extensions.DependencyInjection;
using ChatKid.Application.IServices;
using ChatKid.Application.Services;

namespace ChatKid.Application
{
    public static class Registrations
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IFamilyService, FamilyService>();
            services.AddTransient<IAdminService, AdminService>();
            services.AddTransient<IOtpService, OtpService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAdvertisingService, AdvertisingService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IBlogService, BlogService>();
            services.AddTransient<IExpertService, ExpertService>();
            services.AddTransient<IChannelService, ChannelService>();
            services.AddTransient<IChannelUserService, ChannelUserService>();
            services.AddTransient<IMessageService, MessageService>();
            services.AddTransient<IServiceService, ServiceService>();
            services.AddTransient<IKidServiceService, KidServiceService>();
            services.AddTransient<IQuestionService, QuestionService>();
            services.AddTransient<ISubcriptionService, SubcriptionService>();
            services.AddTransient<IWalletService, WalletService>();
            services.AddTransient<ITypeBlogService, TypeBlogService>();
            services.AddTransient<IDiscussRoomService, DiscussRoomService>();

            services.AddSignalR();
            return services;
        }
    }
}
