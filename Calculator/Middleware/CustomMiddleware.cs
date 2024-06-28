
using Cal.Common.Enum;
using Cal.Common.Helpers;
using Cal.Model.Settings;
using Cal.Model.Token;
using Cal.Service.Account;
using Cal.Service.JWTAuthentication;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Diagnostics;
using System.Net;

namespace Calculator.Middleware
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly IJWTAuthenticationService _jwtAuthenticationService;

        public CustomMiddleware(RequestDelegate next,
           Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IJWTAuthenticationService jwtAuthenticationService
           )
        {
            _next = next;
            _hostingEnvironment = hostingEnvironment;
            _jwtAuthenticationService = jwtAuthenticationService;
        }
        public async Task Invoke(HttpContext context, IAccountService _accountService)
        {
            var directoryPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Logs");

            string headervalue = context.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(headervalue))
            {
                string jwtToken = context.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                if (!string.IsNullOrEmpty(jwtToken))
                {
                    TokenModel userTokenModel = _jwtAuthenticationService.GetUserTokenData(jwtToken);
                    var result = await _accountService.ValidateUserTokenData(userTokenModel.Id, jwtToken, userTokenModel.TokenValidTo);
                    if (result == Convert.ToInt16(TokenEnum.TokenValid))
                    {
                        if (userTokenModel != null)
                        {
                            if (userTokenModel.TokenValidTo < DateTime.UtcNow.AddMinutes(1))
                            {
                                context.Response.ContentType = "application/json";
                                context.Response.Headers[HeaderNames.AccessControlAllowOrigin] = "*";
                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                return;
                            }
                        }
                    }
                    else
                    {
                        context.Response.ContentType = "application/json";
                        context.Response.Headers[HeaderNames.AccessControlAllowOrigin] = "*";
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        return;
                    }
                }
            }

            using (MemoryStream requestBodyStream = new MemoryStream())
            {
                using (MemoryStream responseBodyStream = new MemoryStream())
                {
                    Stream originalRequestBody = context.Request.Body;
                    Stream originalResponseBody = context.Response.Body;

                    try
                    {
                        await context.Request.Body.CopyToAsync(requestBodyStream);
                        requestBodyStream.Seek(0, SeekOrigin.Begin);

                        string requestBodyText = new StreamReader(requestBodyStream).ReadToEnd();

                        requestBodyStream.Seek(0, SeekOrigin.Begin);
                        context.Request.Body = requestBodyStream;

                        string responseBody = "";

                        context.Response.Body = responseBodyStream;

                        Stopwatch watch = Stopwatch.StartNew();
                        await _next(context);
                        watch.Stop();

                        responseBodyStream.Seek(0, SeekOrigin.Begin);
                        responseBody = new StreamReader(responseBodyStream).ReadToEnd();

                        responseBodyStream.Seek(0, SeekOrigin.Begin);

                        await responseBodyStream.CopyToAsync(originalResponseBody);
                    }
                    catch (Exception ex)
                    {
                        await context.Request.Body.CopyToAsync(requestBodyStream);
                        requestBodyStream.Seek(0, SeekOrigin.Begin);

                        string requestBodyText = new StreamReader(requestBodyStream).ReadToEnd();

                        context.Response.ContentType = "application/json";
                        context.Response.Headers[HeaderNames.AccessControlAllowOrigin] = "*";
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        byte[] data = System.Text.Encoding.UTF8.GetBytes(new BaseApiResponse()
                        {
                            Success = false,
                            Message = ex.Message
                        }.ToString());

                        originalResponseBody.WriteAsync(data, 0, data.Length);
                        var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
                        logger.Error(ex.Message,ex);
                        return;
                    }
                    finally
                    {
                        context.Request.Body = originalRequestBody;
                        context.Response.Body = originalResponseBody;
                    }
                }
            }
        }
    }
}
