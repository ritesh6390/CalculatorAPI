using Cal.Model.Login;


namespace Cal.Service.Account
{
    public interface IAccountService
    {
        Task<long> ValidateUserTokenData(long UserId, string jwtToken, DateTime TokenValidDate);
        Task<SaltResponseModel> GetUserSalt(string EmailId);
        Task<LoginResponseModel> LoginUser(LoginRequestModel model);
        Task<long> UpdateLoginToken(string Token, long UserId);
        Task<long> LogoutUser(long UserId);
        Task<long> GetUserIDByEmail(string EmailId);
    }
}
