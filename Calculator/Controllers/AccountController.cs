using Cal.Common.Helpers;
using Cal.Model.Login;
using Cal.Model.Settings;
using Cal.Model.Token;
using Cal.Service.Account;
using Cal.Service.JWTAuthentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.VisualBasic;

namespace Calculator.Controllers
{
    [Route("api/account")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;
        private IConfiguration _config;
        private readonly IJWTAuthenticationService _jwtAuthenticationService;
        private readonly AppSettings _appSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #region Constructor
        public AccountController(
            IAccountService loginService,
            IConfiguration config,
            IJWTAuthenticationService jwtAuthenticationService,
            IOptions<AppSettings> appSettings,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _accountService = loginService;
            _config = config;
            _jwtAuthenticationService = jwtAuthenticationService;
            _appSettings = appSettings.Value;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

      
        [HttpPost]
        [Route("login")]
        public async Task<ApiPostResponse<LoginResponseModel>> LoginUser([FromBody] LoginRequestModel model)
        {
            ApiPostResponse<LoginResponseModel> response = new ApiPostResponse<LoginResponseModel>() { Data = new LoginResponseModel() };
            model.Password = EncryptionDecryption.GetEncrypt(model.Password);

            var res = await _accountService.GetUserSalt(model.EmailId);
            if (res == null)
            {
                response.Success = false;
                response.Message = "Email ID is not valid.";
                return response;
            }
            
            string Hash = EncryptionDecryption.GetDecrypt(res.Password);
            string Salt = EncryptionDecryption.GetDecrypt(res.PasswordSalt);

            bool isPasswordMatched = EncryptionDecryption.Verify(model.Password, Hash, Salt);
            if (isPasswordMatched)
            {
                model.Password = res.Password;
                LoginResponseModel result = await _accountService.LoginUser(model);
                if (result != null && result.UserId > 0)
                {
                    TokenModel objTokenData = new TokenModel();
                    objTokenData.EmailId = model.EmailId;
                    objTokenData.Id = result.UserId != null ? result.UserId : 0;
                    objTokenData.FullName = result.FullName;
                    AccessTokenModel objAccessTokenData = _jwtAuthenticationService.GenerateToken(objTokenData, _appSettings.JWT_Secret, _appSettings.JWT_Validity_Mins);
                    result.JWTToken = objAccessTokenData.Token;

                    await _accountService.UpdateLoginToken(objAccessTokenData.Token, objAccessTokenData.UserId);
                    string host = _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host.Value + "/";

                    response.Message = "Logged in successfully";
                    response.Success = true;
                    response.Data.JWTToken = result.JWTToken.ToString();
                    response.Data.UserId = result.UserId;
                    response.Data.FullName = result.FullName;
                    response.Data.EmailId = result.EmailId;
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Invalid email ID or password";
                    return response;
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Invalid email ID or password";
                return response;
            }
        }
    }
}
