using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cal.Model.Login
{
    public class LoginRequestModel
    {
        [Required(ErrorMessage = "Email id required!")]
        public string EmailId { get; set; }
        [Required(ErrorMessage = "Password required!")]
        public string Password { get; set; }
    }
    public class SaltResponseModel
    {
        public string PasswordSalt { get; set; }
        public string Password { get; set; }
    }
}
