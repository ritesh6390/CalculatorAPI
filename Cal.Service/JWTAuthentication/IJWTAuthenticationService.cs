using Cal.Model.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cal.Service.JWTAuthentication
{
    public interface IJWTAuthenticationService
    {
        AccessTokenModel GenerateToken(TokenModel userToken, string JWT_Secret, int JWT_Validity_Mins);
        TokenModel GetUserTokenData(string jwtToken);
    }
}
