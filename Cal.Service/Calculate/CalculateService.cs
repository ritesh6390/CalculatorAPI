using Cal.Data.Account;
using Cal.Data.Calculator;
using Cal.Model.Calculate;
using Cal.Model.ReqResponse;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cal.Service.Calculate
{
    public class CalculateService : ICalculateService
    {
        #region Fields
        private readonly ICalculatorRepository _repository;
        #endregion

        #region Construtor
        public CalculateService(ICalculatorRepository repository)
        {
            _repository = repository;
        }
        #endregion
        public Task<List<CalculateResponseModel>> GetCalculateHistory()
        {
            return _repository.GetCalculateHistory();
        }

        public Task<ResponseModel> InsertCalculateHistory(CalculateRequestModel model)
        {
            return _repository.InsertCalculateHistory(model);
        }
    }
}
