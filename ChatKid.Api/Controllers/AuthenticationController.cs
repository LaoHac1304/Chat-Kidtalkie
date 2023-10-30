using ChatKid.Api.Services.TokenIssuer;
using ChatKid.Api.Services.Validators;
using ChatKid.ApiFramework.AuthTokenIssuer;
using ChatKid.Application.IServices;
using ChatKid.Application.Models;
using ChatKid.Application.Models.RequestModels;
using ChatKid.Application.Models.ViewModels.AdminViewModels;
using ChatKid.Application.Models.ViewModels.ExpertViewModels;
using ChatKid.Application.Models.ViewModels.FamilyViewModels;
using ChatKid.Common.CommandResult;
using ChatKid.Common.CommandResult.CommandMessages;
using ChatKid.Common.Constants;
using ChatKid.Common.Extensions;
using ChatKid.DataLayer.Identity;
using ChatKid.GoogleServices.GoogleAuthentication;
using ChatKid.GoogleServices.GoogleGmail;
using ChatKid.RedisService.RefreshTokenCaching;
using Google.Apis.Oauth2.v2.Data;
using KMS.Healthcare.TalentInventorySystem.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Data;
using System.Net;

namespace ChatKid.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly ITokenIssuer _tokenIssuer;

        private readonly IAdminService adminService;
        private readonly IGoogleAuthenticationService googleAuthenticationService;
        private readonly IGoogleGmailService googleGmailService;
        private readonly IFamilyService familyService;
        private readonly IExpertService expertService;

        private readonly IHttpContextAccessor httpContextAccessor;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IRefreshTokenCachingService _refreshTokenCaching;

        public AuthenticationController(
            IAdminService adminService,
            IGoogleAuthenticationService googleAuthenticationService,
            IGoogleGmailService googleGmailService,
            IFamilyService familyService,
            IExpertService expertService,
            IHttpContextAccessor httpContextAccessor,
            ITokenIssuer tokenIssuer,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IRefreshTokenCachingService refreshTokenCaching)
        {
            this.adminService = adminService;
            this.googleAuthenticationService = googleAuthenticationService;
            this.googleGmailService = googleGmailService;
            this.familyService = familyService;
            this.expertService = expertService;
            this.httpContextAccessor = httpContextAccessor;
            _tokenIssuer = tokenIssuer;
            _userManager = userManager;
            _roleManager = roleManager;
            _refreshTokenCaching = refreshTokenCaching;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginTokenResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GoogleAuthentication([FromBody] LoginRequest request)
        {
            var validator = new UserLoginValidator().Validate(request);
            if (!validator.IsValid)
            {
                var errors = validator.Errors.Select(error => error.ErrorMessage);
                return BadRequest(errors);
            }

            var response = await HandleGoogleLogin(request.AccessToken);
            if (!response.Succeeded) return StatusCode(response.GetStatusCode(), response.GetData());
            return Ok(response.GetData());
        }

        [HttpPost("register")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GoogleRegister([FromBody] RegisterRequest request)
        {
            var validator = new RegisterValidator().Validate(request);
            if (!validator.IsValid)
            {
                var errors = validator.Errors.Select(error => error.ErrorMessage);
                return BadRequest(errors);
            }

            var response = await HandleGoogleRegister(request.AccessToken);
            if (!response.Succeeded) return StatusCode(response.GetStatusCode(), response.GetData());
            return Ok(new
            {
                VerifyToken = response.GetData(),
            });
        }

        [HttpGet("otp")]
        public async Task<IActionResult> GetOtp([FromQuery] int otp)
        {
            CommandResult response;
            if (otp != 0)
            {
                response = await HandleVerifyOtp(otp);
            }
            else
            {
                if (httpContextAccessor.HttpContext.User is null) return Unauthorized();
                var email = httpContextAccessor.HttpContext.User.Identity!.Name;
                await this.googleGmailService.SendOTP(email);
                return Ok();
            }

            return StatusCode(response.GetStatusCode(), response.GetData());
        }

        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(LoginTokenResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest request)
        {
            var validator = new TokenValidator().Validate(request);
            if (!validator.IsValid)
            {
                var errors = validator.Errors.Select(error => error.ErrorMessage);
                return BadRequest(errors);
            }

            var response = await RefreshToken(request.AccessToken, request.RefreshToken);
            return StatusCode(response.GetStatusCode(), response.GetData());
        }

        [HttpGet("info")]
        [ProducesResponseType(typeof(UserResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(FamilyViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetUserInfo()
        {
            var email = httpContextAccessor.HttpContext!.User.Identity!.Name;
            string role = await GetCurrentRole(email);
            if (role.Equals(UserRoles.SuperAdmin) || role.Equals(UserRoles.Admin))
            {
                AdminViewModel response = null;

                var currentUser = GetInfoCurrentUser();
                if (currentUser is null) return Unauthorized();

                var currentEmail = currentUser.Email;
                var currentAvatar = currentUser.Avatar;

                response = await adminService.GetAdminInfo(currentEmail, currentAvatar);

                if (response is null) NotFound();
                return Ok(response);

            }

            if (role.Equals(UserRoles.Parent))
            {
                var familyUser = await familyService.GetByEmailAsync(email);
                if (familyUser is null) return NotFound();
                return Ok(familyUser);
            }

            if (role.Equals(UserRoles.Expert))
            {
            }


            if (role.Equals(UserRoles.Expert))
            {
                var currentUser = GetInfoCurrentUser();
                if (currentUser is null) return Unauthorized();
                var currentEmail = currentUser.Email;
                var currentAvatar = currentUser.Avatar;
                var response = await expertService.GetExpertInfo(currentEmail, currentAvatar);
                if (response is null) NotFound();
                return Ok(response);

            }
            
            return Unauthorized("Email Or Role Invalid!!!");
        }

        private string EncryptClientIpAddress()
        {
            var model = new
            {
                CreatedAt = DateTime.UtcNow,
                IPAddress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress!.ToString()
            };
            return model.ToString().ToBase62();
        }

        private async Task<string> GetCurrentRole(string email)
        {
            var user = await _userManager.FindByNameAsync(email);
            if (user is null) return UserRoles.Parent;
            var userRoles = await _userManager.GetRolesAsync(user);
            return userRoles.FirstOrDefault();
        }

        private CurrentUserInfoGoogle? GetInfoCurrentUser()
        {
            if (httpContextAccessor.HttpContext is null ||
                httpContextAccessor.HttpContext.User is null)
                return null;

            var token = httpContextAccessor.HttpContext!
                .Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            var principal = _tokenIssuer.GetClaimsPrincipal(token);

            var avatar_url = principal.Claims
                .SingleOrDefault(x => x.Type.Equals(ClaimTypesUrlConstant.AvatarUrl));

            var avatar = avatar_url.ToString();
            avatar = avatar.Replace("avatar_url:", "");

            avatar = avatar.Replace(" ", "");

            var email = httpContextAccessor.HttpContext.User.Identity!.Name;

            var currentUserInfoGoogle = new CurrentUserInfoGoogle
            {
                Email = email,
                Avatar = avatar
            };

            return currentUserInfoGoogle;
        }

        private async Task<CommandResult> HandleGoogleLogin(string token)
        {
            LoginTokenResponse authResponse = null;

            var response = await this.googleAuthenticationService.GoogleLogin(token);
            if (response.Succeeded)
            {
                Userinfo userinfo = (Userinfo)response.Data;
                ClaimModel model = new()
                {
                    Id = userinfo.Id,
                    Email = userinfo.Email,
                    LastName = userinfo.FamilyName,
                    FirstName = userinfo.GivenName,
                    FullName = userinfo.Name,
                    ImageUrl = userinfo.Picture
                };
                if (model == null) return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.Unauthorized,
                    Description = "User Info Is " + HttpMessages.Unauthorized
                });

                var role = await GetCurrentRole(userinfo.Email);
                if (role.Equals(UserRoles.Parent))
                {
                    var familyUser = await familyService.GetByEmailAsync(userinfo.Email);
                    if (familyUser is null) return CommandResult.Failed(new CommandResultError()
                    {
                        Code = (int)HttpStatusCode.Unauthorized,
                        Description = String.Format(CommandMessages.NotRegisted, "User")
                    });
                }

                var claims = await _tokenIssuer.GenerateJwtClaims(model, new[] { role });
                var jwtToken = _tokenIssuer.GenerateAccessToken(claims);

                string refreshToken = EncryptClientIpAddress();

                var result = await _refreshTokenCaching.SaveSync(userinfo.Email, refreshToken);
                if(!result) return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Description = String.Format(CommandMessages.RedisSavedFailed, userinfo.Email + "-" + refreshToken)
                });

                authResponse = new LoginTokenResponse()
                {
                    Token = jwtToken,
                    RefreshToken = refreshToken,
                    Email = userinfo.Email
                };
                return CommandResult.SuccessWithData(authResponse);
            }

            return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.Unauthorized,
                Description = HttpMessages.Unauthorized
            });
        }

        private async Task<CommandResult> HandleGoogleRegister(string token)
        {
            var googleResponse = await this.googleAuthenticationService.GoogleLogin(token);
            if (googleResponse.Succeeded)
            {
                Userinfo userinfo = (Userinfo)googleResponse.GetData();

                var role = await GetCurrentRole(userinfo.Email);
                if (!role.Equals(UserRoles.Parent)) return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.Unauthorized,
                    Description = String.Format(CommandMessages.HasAnotherRole, "User")
                });

                var familyUser = await this.familyService.GetByEmailAsync(userinfo.Email);
                if (familyUser is not null) return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.Unauthorized,
                    Description = "Email Has Been Registed"
                });

                var verifyResponse = await googleGmailService.SendOTP(userinfo.Email);
                if (!verifyResponse.Succeeded) return CommandResult.Failed(new CommandResultError()
                {
                    Code = verifyResponse.GetStatusCode(),
                    Description = verifyResponse.GetData().ToString()
                });

                var claims = await _tokenIssuer.GenerateVerifyToken(userinfo.Email, UserRoles.Parent);
                var jwtToken = _tokenIssuer.GenerateAccessToken(claims);

                return CommandResult.SuccessWithData(jwtToken);
            }

            return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.Unauthorized,
                Description = HttpMessages.Unauthorized
            });
        }

        private async Task<CommandResult> HandleVerifyOtp(int otp)
        {
            var verifyToken = httpContextAccessor.HttpContext!.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            if (verifyToken == "") return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.BadRequest,
                Description = "Verify Token Invalid"
            });

            var principal = _tokenIssuer.GetClaimsPrincipal(verifyToken);
            var scope = principal.Claims.SingleOrDefault(x => x.Type == ClaimTypesUrlConstant.Scope);
            if (scope is null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.Unauthorized,
                Description = "Scope Is Wrong"
            });

            var role = principal.Claims.SingleOrDefault(x => x.Type == ClaimTypesUrlConstant.Role);
            if (role is null) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.Unauthorized,
                Description = "Invalid Role"
            });


            var email = httpContextAccessor.HttpContext.User.Identity!.Name;
            var isVerified = await this.googleGmailService.VerifyOTP(email, otp);
            if (isVerified)
            {
                ClaimModel claimModel = new ClaimModel()
                {
                    Email = email,
                };
                var claims = await _tokenIssuer.GenerateJwtClaims(claimModel, new[] { role!.Value });
                var token = _tokenIssuer.GenerateAccessToken(claims);

                var refreshToken = EncryptClientIpAddress();

                var result = await _refreshTokenCaching.SaveSync(email, refreshToken);
                if (!result) return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Description = String.Format(CommandMessages.RedisSavedFailed, email + "-" + refreshToken)
                });

                LoginTokenResponse authResponse = new LoginTokenResponse()
                {
                    Token = token,
                    RefreshToken = refreshToken
                };
                return CommandResult.SuccessWithData(authResponse);
            }
            return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.Unauthorized,
                Description = "Otp Is Not Matched Or Expired"
            });
        }
        private async Task<CommandResult> RefreshToken(string accessToken, string refreshToken)
        {
            var principal = _tokenIssuer.GetClaimsPrincipal(accessToken);
            var email = principal.Identity!.Name;

            var isValid = await _refreshTokenCaching.IsValidTokenAsync(email, refreshToken);
            if(!isValid) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.Unauthorized,
                Description =String.Format(CommandMessages.NotMatched, refreshToken)
            });

            ClaimModel model = new ClaimModel()
            {
                Email = email,
            };

            var role = principal.Claims
                .SingleOrDefault(x => x.Type.Equals(ClaimTypesUrlConstant.Role));

            var claims = await _tokenIssuer.GenerateJwtClaims(model, new[] { role!.Value });
            var token = _tokenIssuer.GenerateAccessToken(claims);

            var newRefreshToken = EncryptClientIpAddress();

            var result = await _refreshTokenCaching.SaveSync(email, newRefreshToken);
            if (!result) return CommandResult.Failed(new CommandResultError()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Description = String.Format(CommandMessages.RedisSavedFailed, email + "-" + refreshToken)
            });

            var authResponse = new LoginTokenResponse()
            {
                Token = token,
                RefreshToken = newRefreshToken,
                Email = principal.Identity.Name,
            };
            return CommandResult.SuccessWithData(authResponse);
        }
    }
}
