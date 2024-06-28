using Cal.Model.Calculate;
using Cal.Model.Config;
using Cal.Model.Login;
using Cal.Model.ReqResponse;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cal.Data.Calculator
{
    public class CalculatorRepository : BaseRepository, ICalculatorRepository
    {
        #region Fields
        private readonly DataConfig _dataConfig;
        #endregion

        #region Constructor
        public CalculatorRepository(IOptions<DataConfig> dataConfig) : base(dataConfig)
        {
            _dataConfig = dataConfig.Value;
        }
        #endregion

        public async Task<ResponseModel> InsertCalculateHistory(CalculateRequestModel model)
        {
            var param = new DynamicParameters();
            param.Add("@FirstValue", model.FirstValue);
            param.Add("@SecondValue", model.SecondValue);
            param.Add("@CalculateValue", model.CalculateValue);
            param.Add("@Operator", model.Operator);
            var result = await QueryFirstOrDefaultAsync<ResponseModel>("SP_Insert_Calculate_History", param, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<List<CalculateResponseModel>> GetCalculateHistory()
        {
            var data = await QueryAsync<CalculateResponseModel>("SP_Get_Calculate_History", commandType: CommandType.StoredProcedure);
            return data.ToList();
        }
    }
}
