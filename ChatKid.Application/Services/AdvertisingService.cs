using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.FilterModels;
using ChatKid.Application.Models.RequestModels.Advertising;
using ChatKid.Application.Models.ViewModels;
using ChatKid.Application.Models.ViewModels.AdvertisingViewModels;
using ChatKid.Common.CommandResult;
using ChatKid.Common.CommandResult.CommandMessages;
using ChatKid.Common.Extensions;
using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Repositories;
using ChatKid.DataLayer.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ChatKid.Application.Services
{
    public class AdvertisingService : IAdvertisingService
    {
        private readonly IAdvertisingRepository _advertisingRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public AdvertisingService(IAdvertisingRepository advertisingRepository, IAdminRepository adminRepository,IHttpContextAccessor httpContextAccessor , IMapper mapper)
        {
            _advertisingRepository = advertisingRepository;
            _adminRepository = adminRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<CommandResult> AddAdvertisingAsync(AdvertisingCreateRequest model)
        {
            var advertising = _mapper.Map<Advertising>(model);  
            if (advertising == null)
            {
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.BadRequest,
                    Description = "Advertising is null"
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
            if (currentUser is null)
            {
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.Unauthorized,
                    Description = "Unauthorzied"
                });
            }
            advertising.CreatedBy = currentUser.Id;
            var result = await _advertisingRepository.InsertAsync(advertising);
            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = string.Format(CommandMessages.CreateFailed, advertising.Id)
            });
            return CommandResult.Success;
        }

        public async Task<CommandResult> UpdateAdvertisingAsync(Guid id, AdvertisingUpdateRequest model)
        {
            var advertising = await _advertisingRepository.GetByIdAsync(id);
            if (advertising == null)
            {
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.NotFound,
                    Description = string.Format(CommandMessages.NotFound, id)
                }) ;
            }
            _mapper.Map(model, advertising);
            var result = await _advertisingRepository.UpdateAsync(advertising);
            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = string.Format(CommandMessages.UpdateFailed, id)
            });
            return CommandResult.Success;   

        }

        public async Task<CommandResult> ClickAdvertisingAsync(Guid id)
        {
            var advertising = await _advertisingRepository.GetByIdAsync(id);
            if (advertising == null)
            {
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.NotFound,
                    Description = string.Format(CommandMessages.NotFound, id)
                });
            }
            advertising.Clicks++;
            var result = await _advertisingRepository.UpdateAsync(advertising);
            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = string.Format(CommandMessages.UpdateFailed, id)

            });
            return CommandResult.Success;
        }

        public async Task<CommandResult> DeleteAdvertisingAsync(Guid id)
        {
            var advertising = await _advertisingRepository.GetByIdAsync(id);
            if (advertising is null) 
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.NotFound,
                    Description = string.Format(CommandMessages.NotFound, id)
                });
            advertising.Status = 0;
            bool result = await _advertisingRepository.UpdateAsync(advertising);
            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = string.Format(CommandMessages.DeleteFailed, advertising.Id)
            });
            return CommandResult.Success;
        }

        public async Task<AdvertisingDetailViewModel> GetAdvertisingAsync(Guid id)
        {
            var advertising = await _advertisingRepository.TableNoTracking.Where(x => x.Id.Equals(id)).Include(x => x.CreateAdmin).FirstOrDefaultAsync();
            var model = _mapper.Map<AdvertisingDetailViewModel>(advertising);
            return model;
        }

        public async Task<List<AdvertisingViewModel>> GetAllAdvertisingAsync(string search)
        {
            var advertisings = await _advertisingRepository.TableNoTracking
                .Where(x => EF.Functions.ToTsVector(""+x.Content)
                .Matches(EF.Functions.ToTsQuery(search)))
                .ToListAsync();
            var model = _mapper.Map<List<AdvertisingViewModel>>(advertisings);
            return model;
        }

        public async Task<(int, List<AdvertisingDetailViewModel>)> 

            GetAdvertisingPagesAsync(FilterViewModel filterViewModel, AdvertisingFilter advertisingFilter, int pageIndex, int pageSize, string sortBy)
        {
            IQueryable<Advertising> advertisings;
            if (advertisingFilter.isRandom)
            {
                var type = advertisingFilter.Type ?? "";
                var now = DateTime.Now;
                var temp = await _advertisingRepository.TableNoTracking
                    .Where(x => x.Status == 1 && x.StartDate <= now && x.EndDate >= now && x.Type.Equals(type)).ToListAsync();
                List<Advertising> res = new List<Advertising>();
                if (temp.Count > 0)
                {
                    Random rnd = new Random();
                    int num = rnd.Next(temp.Count);
                    res = temp.GetRange(num, 1);
                }
                return (res.Count, _mapper.Map<List<AdvertisingDetailViewModel>>(res));
            }
            var search = filterViewModel.SearchString ?? "";

            if (search.IsNullOrEmpty())
            {
                advertisings = _advertisingRepository.TableNoTracking;
            }

            else
            {
                advertisings = _advertisingRepository.TableNoTracking
                                .Where(ad => EF.Functions.ToTsVector("english", "" + ad.Content)
                                .Matches(EF.Functions.ToTsQuery("english", search)))
                                .OrderByDescending
                                (ad => EF.Functions.ToTsVector("english", "" + ad.Content)
                                .Rank(EF.Functions.ToTsQuery("english", search)));
            }


            if (filterViewModel.Status != 2) advertisings = advertisings.Where(x => x.Status == filterViewModel.Status);
            if (!sortBy.IsNullOrEmpty())
            {
                advertisings = advertisings.Sort(sortBy);
            }

            //.ToList();
            advertisings = advertisings.Include(x => x.CreateAdmin);
            var result = await advertisings.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            return ((advertisings.Count(), _mapper.Map<List<AdvertisingDetailViewModel>>(result)));
        }

    }
}
