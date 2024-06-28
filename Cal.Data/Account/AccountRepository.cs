using Cal.Model.Config;
using Cal.Model.Login;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Data;

namespace Cal.Data.Account
{
    public class AccountRepository : BaseRepository, IAccountRepository
    {
        #region Fields
        private IConfiguration _config;
        #endregion

        #region Constructor
        public AccountRepository(IConfiguration config, IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
            _config = config;
        }
        #endregion

        #region Post

        public async Task<long> ValidateUserTokenData(long UserId, string jwtToken, DateTime TokenValidDate)
        {
            var param = new DynamicParameters();
            param.Add("@UserId", UserId);
            return await QueryFirstOrDefaultAsync<long>("sp_ValidateToken", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<SaltResponseModel> GetUserSalt(string EmailId)
        {
            var param = new DynamicParameters();
            param.Add("@EmailId", EmailId);
            var data = await QueryFirstOrDefaultAsync<SaltResponseModel>("sp_GetUserSaltByEmail", param, commandType: CommandType.StoredProcedure);
            return data;
        }

        public async Task<LoginResponseModel> LoginUser(LoginRequestModel model)
        {
            var param = new DynamicParameters();
            param.Add("@EmailId", model.EmailId);
            param.Add("@Password", model.Password);
            return await QueryFirstOrDefaultAsync<LoginResponseModel>("sp_UserLogin", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<long> UpdateLoginToken(string Token, long UserId)
        {
            var param = new DynamicParameters();
            param.Add("@Token", Token);
            param.Add("@UserId", UserId);
            return await QueryFirstOrDefaultAsync<long>("SP_UpdateLoginToken", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<long> LogoutUser(long UserId)
        {
            var param = new DynamicParameters();
            param.Add("@UserId", UserId);
            return await QueryFirstOrDefaultAsync<long>("sp_LogoutUser", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<long> GetUserIDByEmail(string EmailId)
        {
            var param = new DynamicParameters();
            param.Add("@EmailId", EmailId);
            return await QueryFirstOrDefaultAsync<long>("sp_GetUserIDByEmail", param, commandType: CommandType.StoredProcedure);
        }

        #endregion
    }
}
