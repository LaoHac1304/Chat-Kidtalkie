using AutoMapper;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.ViewModels.OtpViewModels;
using ChatKid.Common.CommandResult;
using ChatKid.Common.CommandResult.CommandMessages;
using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ChatKid.Application.Services
{
    public class OtpService : IOtpService
    {
        private readonly IOtpRepository otpRepository;
        private readonly IMapper _mapper;
        public OtpService(IOtpRepository otpRepository, IMapper mapper)
        {
            this.otpRepository = otpRepository;
            _mapper = mapper;
        }
        public async Task<bool> CheckOtp(string email, int otp)
        {
            var model = await otpRepository.TableNoTracking.SingleOrDefaultAsync(x => x.Email.Equals(email));
            return otp == model.OTP;
        }

        public async Task<CommandResult> CreateOtp(string email, int otp)
        {
            var model = new OtpViewModel
            {
                Email = email,
                OTP = otp,
            };
            bool result = await otpRepository.InsertAsync(_mapper.Map<Otp>(model));
            if(!result) return CommandResult.Failed(new CommandResultError
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = String.Format(CommandMessages.CreateFailed, email + " " + otp)
            });
            return CommandResult.Success;
        }

        public async Task<List<OtpViewModel>> GetOtpsAsync()
        {
            var result = await this.otpRepository.TableNoTracking.ToListAsync();
            return _mapper.Map<List<OtpViewModel>>(result);
        }

        public async Task<CommandResult> UpdateOtp(string email, int otp)
        {
            var model = await otpRepository.TableNoTracking.SingleOrDefaultAsync(x => x.Email.Equals(email));
            if(model is null)
            {
                bool insertResult = await otpRepository.InsertAsync(_mapper.Map<Otp>(new OtpViewModel
                {
                    Email = email,
                    OTP = otp,
                }));

                if (insertResult) return CommandResult.Success;
            }
            else
            {
                var entity = new OtpViewModel
                {
                    Id = model.Id,
                    Email = model.Email,
                    OTP = otp,
                };
                bool result = await otpRepository.UpdateAsync(_mapper.Map<Otp>(entity));
                if (result) return CommandResult.Success;
            }

            return CommandResult.Failed(new CommandResultError
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = String.Format(CommandMessages.UpdateFailed, email + " " + otp)
            });
        }
    }
}
