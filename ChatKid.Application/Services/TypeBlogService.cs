using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.RequestModels.TypeBlogRequests;
using ChatKid.Application.Models.ViewModels.TypeBlogViewModels;
using ChatKid.Common.CommandResult;
using ChatKid.Common.CommandResult.CommandMessages;
using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ChatKid.Application.Services
{
    public class TypeBlogService : ITypeBlogService
    {
        private readonly ITypeBlogRepository _typeBlogRepository;
        private readonly IMapper _mapper;

        public TypeBlogService(ITypeBlogRepository typeBlogRepository, IMapper mapper)
        {
            _typeBlogRepository = typeBlogRepository;
            _mapper = mapper;
        }

        public async Task<CommandResult> CreateTypeBlog(TypeBlogCreateRequest typeBlogViewModel)
        {
            var blogType = _mapper.Map<TypeBlog>(typeBlogViewModel);
            if (blogType == null)
            {
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.NotFound,
                    Description = "Blog type is null"
                });
            }
            var result = await _typeBlogRepository.InsertAsync(blogType);
            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = "Insert Advertising Failed"
            });
            return CommandResult.Success;
        }

        public async Task<CommandResult> DeleteTypeBlog(Guid id)
        {
            var blogType = await _typeBlogRepository.GetByIdAsync(id);
            if (blogType == null)
            {
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.NotFound,
                    Description = string.Format(CommandMessages.NotFound, id)
                });
            }

            var result = await _typeBlogRepository.DeleteAsync(blogType);
            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = string.Format(CommandMessages.DeleteFailed, id)
            });
            return CommandResult.Success;
        }

        public async Task<List<TypeBlogViewModel>> GetAllTypeBlog()
        {
            var typeBlogs = await _typeBlogRepository.TableNoTracking.ToListAsync();
            return _mapper.Map<List<TypeBlogViewModel>>(typeBlogs);
        }

        public async Task<TypeBlogViewModel> GetTypeBlogById(Guid id)
        {
            var typeBlog = await _typeBlogRepository.GetByIdAsync(id);
            return _mapper.Map<TypeBlogViewModel>(typeBlog);
        }

        public async Task<CommandResult> UpdateTypeBlog(Guid id, TypeBlogUpdateRequest typeBlogViewModel)
        {
            var blogType = await _typeBlogRepository.GetByIdAsync(id);
            if (blogType == null)
            {
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.NotFound,
                    Description = string.Format(CommandMessages.NotFound, id)
                });
            }
            _mapper.Map(typeBlogViewModel, blogType);
            var result = await _typeBlogRepository.UpdateAsync(blogType);
            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = string.Format(CommandMessages.UpdateFailed, id)
            });
            return CommandResult.Success;
        }

    }
}
