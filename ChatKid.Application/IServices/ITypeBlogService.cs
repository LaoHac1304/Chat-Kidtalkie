using ChatKid.Application.Models.RequestModels.TypeBlogRequests;
using ChatKid.Application.Models.ViewModels.TypeBlogViewModels;
using ChatKid.Common.CommandResult;

namespace ChatKid.Application.IServices
{
    public interface ITypeBlogService
    {
        Task<List<TypeBlogViewModel>> GetAllTypeBlog();
        Task<TypeBlogViewModel> GetTypeBlogById(Guid id);
        Task<CommandResult> CreateTypeBlog(TypeBlogCreateRequest typeBlogViewModel);
        Task<CommandResult> UpdateTypeBlog(Guid id, TypeBlogUpdateRequest typeBlogViewModel);
        Task<CommandResult> DeleteTypeBlog(Guid id);
    }
}
