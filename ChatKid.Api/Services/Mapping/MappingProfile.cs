using AutoMapper;
using ChatKid.Application.Models.RequestModels.Admin;
using ChatKid.Application.Models.RequestModels.Advertising;
using ChatKid.Application.Models.RequestModels.BlogRequests;
using ChatKid.Application.Models.RequestModels.ChannelRequests;
using ChatKid.Application.Models.RequestModels.ChannelUserRequests;
using ChatKid.Application.Models.RequestModels.DiscussRoom;
using ChatKid.Application.Models.RequestModels.ExpertRequests;
using ChatKid.Application.Models.RequestModels.FamilyRequests;
using ChatKid.Application.Models.RequestModels.KidServiceRequests;
using ChatKid.Application.Models.RequestModels.MessageRequests;
using ChatKid.Application.Models.RequestModels.NotificationRequests;
using ChatKid.Application.Models.RequestModels.QuestionRequests;
using ChatKid.Application.Models.RequestModels.ServiceRequests;
using ChatKid.Application.Models.RequestModels.SubcriptionRequests;
using ChatKid.Application.Models.RequestModels.TypeBlogRequests;
using ChatKid.Application.Models.RequestModels.UserProfileRequests;
using ChatKid.Application.Models.RequestModels.WalletRequests;
using ChatKid.Application.Models.SearchFilter;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.AdminViewModels;
using ChatKid.Application.Models.ViewModels.AdvertisingViewModels;
using ChatKid.Application.Models.ViewModels.BlogViewModels;
using ChatKid.Application.Models.ViewModels.ChannelUserViewModels;
using ChatKid.Application.Models.ViewModels.ChannelViewModels;
using ChatKid.Application.Models.ViewModels.DiscussRoomViewModels;
using ChatKid.Application.Models.ViewModels.ExpertViewModels;
using ChatKid.Application.Models.ViewModels.FamilyViewModels;
using ChatKid.Application.Models.ViewModels.KidServiceViewModel;
using ChatKid.Application.Models.ViewModels.MessageViewModels;
using ChatKid.Application.Models.ViewModels.NotificationViewModels;
using ChatKid.Application.Models.ViewModels.OtpViewModels;
using ChatKid.Application.Models.ViewModels.QuestionViewModels;
using ChatKid.Application.Models.ViewModels.ServiceViewModel;
using ChatKid.Application.Models.ViewModels.SubcriptionViewModels;
using ChatKid.Application.Models.ViewModels.TypeBlogViewModels;
using ChatKid.Application.Models.ViewModels.UserViewModels;
using ChatKid.Application.Models.ViewModels.WalletViewModels;
using ChatKid.DataLayer.Entities;

namespace ChatKid.Api.Services.Mapping
{
    public class MappingProfile : Profile
    {
        //<summary>CreateMap<Source, Destination></summary>
        public MappingProfile() {
            CreateMap<SearchFilter, FilterViewModel>();

            RegisterFamilyMapping();
            RegisterUserMapping();
            RegisterOtp();
            RegisterAdminMapping();
            RegisterAdvertisingMapping();
            RegisterBlogMapping();
            RegisterDiscussRoomMapping();
            RegisterServiceMapping();
            RegisterKidServiceMapping();
            RegisterQuestionMapping();
            RegisterNotificationMapping();
            RegisterExpertMapping();
            RegisterTypeBlogMapping();
            RegisterSubcription();
            RegisterWallet();
        }
        private void RegisterFamilyMapping()
        {
            CreateMap<Family, FamilyViewModel>();
            CreateMap<FamilyViewModel, Family>().IgnoreNull();
            CreateMap<FamilyCreateRequest, FamilyViewModel>();
            CreateMap<FamilyUpdateRequest, FamilyViewModel>();

            CreateMap<Channel, ChannelViewModel>();
            CreateMap<ChannelViewModel, Channel>();
            CreateMap<Channel, ChannelCreateViewModel>();
            CreateMap<ChannelCreateViewModel, Channel>();
            CreateMap<ChannelCreateRequest, ChannelCreateViewModel>();
            CreateMap<ChannelUpdateRequest, ChannelCreateViewModel>();

            CreateMap<ChannelUserViewModel, ChannelUser>();
            CreateMap<ChannelUser, ChannelUserViewModel>();
            CreateMap<ChannelUserRequest, ChannelUserViewModel>();
            CreateMap<ChannelUserViewModel, ChannelUserRequest>();

            CreateMap<Message, MessageViewModel>();
            CreateMap<MessageViewModel, Message>();
            CreateMap<MessageCreateRequest, MessageViewModel>();
            CreateMap<MessageUpdateRequest, MessageViewModel>();

        }
        private void RegisterUserMapping()
        {
            CreateMap<Family, UserFamilyViewModel>()
                .ForMember(dest => dest.FamilyName, opt => opt.MapFrom(src => src.Name));
            CreateMap<User, UserFamilyViewModel>();
            CreateMap<UserViewModel, User>().IgnoreNull();
            CreateMap<User, UserViewModel>();
            CreateMap<UserProfileCreateRequests, UserViewModel>();
            CreateMap<UserProfileUpdateRequests, UserViewModel>();
            CreateMap<AddUsersRequest, AddUsersViewModel>();
        }
        private void RegisterOtp()
        {
            CreateMap<OtpViewModel, Otp>();
            CreateMap<Otp, OtpViewModel>();
        }

        private void RegisterExpertMapping()
        {
            CreateMap<Expert, ExpertViewModel>();
            CreateMap<ExpertCreateRequest, Expert>();
            CreateMap<ExpertUpdateRequest, Expert>().IgnoreNull();
        }

        private void RegisterAdvertisingMapping()
        {
            CreateMap<Advertising, AdvertisingViewModel>();
            CreateMap<Advertising, AdvertisingDetailViewModel>();
            CreateMap<AdvertisingCreateRequest, Advertising>();
            CreateMap<AdvertisingUpdateRequest, Advertising>().IgnoreNull();
        }

        private void RegisterBlogMapping()
        {
            CreateMap<Blog, BlogViewModel>();
            CreateMap<BlogCreateRequest, Blog>();
            CreateMap<BlogUpdateRequest, Blog>().IgnoreNull();
        }

        private void RegisterServiceMapping()
        {
            CreateMap<Service, ServiceViewModel>();
            CreateMap<ServiceCreateRequest, Service>();
            CreateMap<ServiceUpdateRequest, Service>().IgnoreNull();
        }

        private void RegisterKidServiceMapping()
        {
            CreateMap<KidService, KidServiceViewModel>();
            CreateMap<KidServiceCreateRequest, KidService>();
            CreateMap<KidServiceUpdateRequest, KidService>().IgnoreNull();
        }

        private void RegisterQuestionMapping()
        {
            CreateMap<Question, QuestionViewModel>();
            CreateMap<QuestionCreateRequest, Question>();
            CreateMap<QuestionUpdateRequest, Question>().IgnoreNull();
        }

        private void RegisterNotificationMapping()
        {
            CreateMap<Notification, NotificationViewModel>().ForMember(dest => dest.CreatorEmail, opt => opt.MapFrom(src => src.CreateAdmin.Gmail));
            CreateMap<NotificationCreateRequest, Notification>();
            CreateMap<NotificationUpdateRequest, Notification>().IgnoreNull();
        }

        private void RegisterTypeBlogMapping()
        {
            CreateMap<TypeBlog, TypeBlogViewModel>();
            CreateMap<TypeBlogCreateRequest, TypeBlog>();
            CreateMap<TypeBlogUpdateRequest, TypeBlog>().IgnoreNull();
        }

        private void RegisterAdminMapping()
        {
            CreateMap<Admin, AdminViewModel>();
            CreateMap<AdminCreateRequest, Admin>();
            CreateMap<AdminUpdateRequest, Admin>().IgnoreNull();

        }
        private void RegisterDiscussRoomMapping()
        {
            CreateMap<DiscussRoom, DiscussRoomViewModel>();
            CreateMap<DiscussRoomCreateRequest, DiscussRoom>();
            CreateMap<DiscussRoomUpdateRequest, DiscussRoom>().IgnoreNull();

        }

        private void RegisterSubcription()
        {
            CreateMap<Subcription, SubcriptionViewModel>();
            CreateMap<SubcriptionViewModel, Subcription>().IgnoreNull();
            CreateMap<SubcriptionCreateRequest, Subcription>();
            CreateMap<SubcriptionUpdateRequest, Subcription>().IgnoreNull();
            CreateMap<SubcriptionCreateRequest, SubcriptionViewModel>();
            CreateMap<SubcriptionUpdateRequest, SubcriptionViewModel>();
        }
        private void RegisterWallet()
        {
            CreateMap<Wallet, WalletViewModel>();
            CreateMap<WalletViewModel, Wallet>().IgnoreNull();
            CreateMap<WalletCreateRequest, Wallet>();
            CreateMap<WalletUpdateRequest, Wallet>().IgnoreNull();
            CreateMap<WalletCreateRequest, WalletViewModel>();
            CreateMap<WalletUpdateRequest, WalletViewModel>();
        }
    }
}
