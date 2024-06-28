using Cal.Data.Account;
using Cal.Data.Calculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cal.Data
{
    public static class DataRegister
    {
        public static Dictionary<Type, Type> GetTypes()
        {
            var dataDictionary = new Dictionary<Type, Type>
            {
                { typeof(IAccountRepository), typeof(AccountRepository) },
                { typeof(ICalculatorRepository), typeof(CalculatorRepository) }
            };
            return dataDictionary;
        }
    }
}
