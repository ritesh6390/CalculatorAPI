using Cal.Model.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cal.Data.Account
{
    public interface IAccountRepository
    {
        Task<long> ValidateUserTokenData(long UserId, string jwtToken, DateTime TokenValidDate);
        Task<SaltResponseModel> GetUserSalt(string EmailId);
        Task<long> LogoutUser(long UserId);
        Task<LoginResponseModel> LoginUser(LoginRequestModel model);
        Task<long> UpdateLoginToken(string Token, long UserId);
        Task<long> GetUserIDByEmail(string EmailId);
    }
}
