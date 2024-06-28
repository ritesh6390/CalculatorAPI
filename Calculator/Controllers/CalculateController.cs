using Cal.Common.Helpers;
using Cal.Model.Calculate;
using Cal.Model.Config;
using Cal.Model.Settings;
using Cal.Model.Token;
using Cal.Service.Calculate;
using Cal.Service.JWTAuthentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Calculator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculateController : ControllerBase
    {

        #region Fields
        private readonly AppSettings _appSettings;
        private readonly ICalculateService _calculateService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJWTAuthenticationService _jwtAuthenticationService;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly DataConfig _dataConfig;
        private IConfiguration _config;
        #endregion

        #region Constructor
        public CalculateController(ICalculateService calculateService, IHttpContextAccessor httpContextAccessor, IJWTAuthenticationService jwtAuthenticationService, IOptions<AppSettings> appSettings, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IOptions<DataConfig> dataConfig, IConfiguration config)
        {
            _calculateService = calculateService;
            _appSettings = appSettings.Value;
            _httpContextAccessor = httpContextAccessor;
            _jwtAuthenticationService = jwtAuthenticationService;
            _hostingEnvironment = hostingEnvironment;
            _dataConfig = dataConfig.Value;
            _config = config;

        }
        #endregion


        [HttpPost("save")]
        [AllowAnonymous]
        public async Task<BaseApiResponse> InsertCalculateHistory( CalculateRequestModel model)
        {
            BaseApiResponse response = new BaseApiResponse();
            var result = await _calculateService.InsertCalculateHistory(model);
            if (result.IsSuccess == 1)
            {
                response.Message = result.Result;
                response.Success = true;
            }
            else
            {
                response.Message = result.Result;
                response.Success = false;
            }
            return response;
        }
        [HttpGet("list")]
        public async Task<ApiResponse<CalculateResponseModel>> GetCalculateHistory()
        {
            ApiResponse<CalculateResponseModel> response = new ApiResponse<CalculateResponseModel>() { Data = new List<CalculateResponseModel>() };

            var result = await _calculateService.GetCalculateHistory();
            if (result != null)
            {
                response.Data = result;
            }
            response.Success = true;
            return response;
        }
    }
}
