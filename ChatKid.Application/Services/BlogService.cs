using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.RequestModels.BlogRequests;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.BlogViewModels;
using ChatKid.Common.CommandResult;
using ChatKid.Common.CommandResult.CommandMessages;
using ChatKid.Common.Extensions;
using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ChatKid.Application.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly ITypeBlogRepository _typeBlogRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public BlogService(IBlogRepository blogRepository, IAdminRepository adminRepository, ITypeBlogRepository typeBlogRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _blogRepository = blogRepository;
            this._mapper = mapper;
            _adminRepository = adminRepository;
            _typeBlogRepository = typeBlogRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<BlogViewModel>> GetAllBlogAsync(string search)
        {
            var blogs = await _blogRepository.TableNoTracking.Where(x => x.Title.Contains(search) || x.Content.Contains(search)).ToListAsync();
            return _mapper.Map<List<BlogViewModel>>(blogs);
        }

        public async Task<(int, List<BlogViewModel>)> GetBlogPagesAsync(FilterViewModel filter, int pageIndex, int pageSize, string? sort)
        {

            IQueryable<Blog> blogs;
            string search = filter.SearchString ?? "";
            if (search.IsNullOrEmpty())
            {
                blogs = _blogRepository.TableNoTracking;
            }

            else
            {
                blogs = _blogRepository.TableNoTracking
                                .Where(blog => EF.Functions.ToTsVector("english", blog.Title + " " + blog.Content)
                                .Matches(EF.Functions.ToTsQuery("english", search)))
                                .OrderByDescending
                                (ad => EF.Functions.ToTsVector("english", ad.Title + " " + ad.Content)
                                .Rank(EF.Functions.ToTsQuery("english", search)));
            }
            if (filter.Status != 2) blogs = blogs.Where(x => x.Status == filter.Status);
            if (!sort.IsNullOrEmpty())
            {
                blogs = blogs.Sort(sort);
            }
            else
            {
                blogs = blogs.OrderByDescending(x => x.UpdatedAt);
            }
            blogs = blogs.Include(x => x.TypeBlog);
            blogs = blogs.Include(x => x.CreateAdmin);
            var result = await blogs.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            return (blogs.Count(), _mapper.Map<List<BlogViewModel>>(result));
        }

        public async Task<BlogViewModel> GetBlogByIdAsync(Guid id)
        {
            var blog = await _blogRepository.TableNoTracking.Where(x => x.Id.Equals(id)).Include(x => x.TypeBlog).Include(x => x.CreateAdmin).FirstOrDefaultAsync();
            return _mapper.Map<BlogViewModel>(blog); 
        }
        public async Task<CommandResult> AddBlogAsync(BlogCreateRequest model)
        {
            var blog = _mapper.Map<Blog>(model);
            if (blog == null)
            {
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.BadRequest,
                    Description = "Blog is null"
                });
            }
            if (_httpContextAccessor.HttpContext.User is null)
            {
                return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.Unauthorized,
                Description = string.Format("Unauthorzied")
            });
            }

            var currentUser = await _adminRepository.GetAdminByEmailAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
            if (await _typeBlogRepository.GetByIdAsync(blog.TypeBlogId) == null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.NotFound,
                Description = string.Format(CommandMessages.NotFound, "TypeBlog")
            });
            if (currentUser == null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.NotFound,
                Description = string.Format(CommandMessages.NotFound, "Admin")
            });
            blog.CreatedBy = currentUser.Id;
            var result = await _blogRepository.InsertAsync(blog);
            if (!result) 
                return CommandResult.Failed(new CommandResultError()
                {
                Code = (int)HttpStatusCode.BadRequest,
                Description = string.Format(string.Format(CommandMessages.CreateFailed, blog.Id))
            });
            return CommandResult.Success;
        }

        public async Task<CommandResult> DeleteBlogAsync(Guid id)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.NotFound,
                Description = string.Format(CommandMessages.NotFound, id)
            });
            blog.Status = 0;
            var result = await _blogRepository.UpdateAsync(blog);
            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = string.Format(CommandMessages.DeleteFailed, id)
            });
            return CommandResult.Success;
        }

        public async Task<CommandResult> UpdateBlogAsync(Guid id, BlogUpdateRequest model)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.NotFound,
                Description = string.Format(CommandMessages.NotFound, id)
            });
            if (model.TypeBlogId != null && await _typeBlogRepository.GetByIdAsync(model.TypeBlogId) == null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.NotFound,
                Description = string.Format(CommandMessages.NotFound, "TypeBlog")
            });
            _mapper.Map(model, blog);
            var result = await _blogRepository.UpdateAsync(blog);
            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = string.Format(CommandMessages.UpdateFailed, id)
            });
            return CommandResult.Success;
        }

       
    }
}
