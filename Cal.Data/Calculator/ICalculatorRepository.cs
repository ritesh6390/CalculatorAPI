﻿using Cal.Model.Calculate;
using Cal.Model.ReqResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cal.Data.Calculator
{
    public interface ICalculatorRepository
    {
        Task<ResponseModel> InsertCalculateHistory(CalculateRequestModel model);
        Task<List<CalculateResponseModel>> GetCalculateHistory();
    }
}
