using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cal.Model.Calculate
{
    public class CalculateRequestModel
    {
        [Required(ErrorMessage = "First Value required!")]
        public decimal FirstValue { get; set; }
        [Required(ErrorMessage = "Second Value required!")]
        public decimal SecondValue { get; set; }

        [Required(ErrorMessage = "Calculate Value required!")]
        public decimal CalculateValue { get; set; }

        [Required(ErrorMessage = "Operator required")]
        public string? Operator { get; set; }
    }
}
