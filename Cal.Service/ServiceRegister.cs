using Cal.Service.Account;
using Cal.Service.Calculate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cal.Service
{
    public static class ServiceRegister
    {
        public static Dictionary<Type, Type> GetTypes()
        {
            var serviceDictonary = new Dictionary<Type, Type>
            {
                { typeof(IAccountService), typeof(AccountService) },
                { typeof(ICalculateService), typeof(CalculateService) }
            };
            return serviceDictonary;
        }
    }
}
