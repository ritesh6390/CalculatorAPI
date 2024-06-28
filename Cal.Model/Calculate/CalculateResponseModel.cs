using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cal.Model.Calculate
{
    public class CalculateResponseModel
    {
        public decimal FirstValue { get; set; }
        public decimal SecondValue { get; set; }
        public decimal CalculateValue { get; set; }
        public string Operator { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
