using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cal.Model.Token
{
    public class TokenModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string EmailId { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public DateTime TokenValidTo { get; set; }
    }
}
