using Cal.Data.Account;
using Cal.Model.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cal.Service.Account
{
    public class AccountService : IAccountService
    {
        #region Fields
        private readonly IAccountRepository _repository;
        #endregion

        #region Construtor
        public AccountService(IAccountRepository repository)
        {
            _repository = repository;
        }
        #endregion

        public async Task<long> ValidateUserTokenData(long UserId, string jwtToken, DateTime TokenValidDate)
        {
            return await _repository.ValidateUserTokenData(UserId, jwtToken, TokenValidDate);
        }

        public async Task<SaltResponseModel> GetUserSalt(string EmailId)
        {
            return await _repository.GetUserSalt(EmailId);
        }
        public async Task<LoginResponseModel> LoginUser(LoginRequestModel model)
        {
            return await _repository.LoginUser(model);
        }
        public async Task<long> UpdateLoginToken(string Token, long UserId)
        {
            return await _repository.UpdateLoginToken(Token, UserId);
        }

        public async Task<long> LogoutUser(long UserId)
        {
            return await _repository.LogoutUser(UserId);
        }
       
        public async Task<long> GetUserIDByEmail(string EmailId)
        {
            return await _repository.GetUserIDByEmail(EmailId);
        }

       
    }
}
