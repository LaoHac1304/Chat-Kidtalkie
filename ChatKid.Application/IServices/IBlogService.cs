using ChatKid.Application.Models.RequestModels.BlogRequests;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.BlogViewModels;
using ChatKid.Common.CommandResult;

namespace ChatKid.Application.IServices
{
    public interface IBlogService
    {
        public Task<CommandResult> AddBlogAsync(BlogCreateRequest model);
        public Task<CommandResult> DeleteBlogAsync(Guid id);
        public Task<CommandResult> UpdateBlogAsync(Guid id, BlogUpdateRequest model);
        public Task<BlogViewModel> GetBlogByIdAsync(Guid id);
        public Task<List<BlogViewModel>> GetAllBlogAsync(string search);
        public Task<(int, List<BlogViewModel>)> GetBlogPagesAsync(FilterViewModel filterViewModel, int pageIndex, int pageSize, string? sort);
    }
}
