using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PremiumCalculator.Model
{
    public class UserData
    {
        //[DataType(DataType.PhoneNumber)]
        [Display(Name = "Death Cover Amount")]              
        [Required(AllowEmptyStrings = false, ErrorMessage = "DeathCoverAmount should not be blank")]
        [Range(0, 99999999, ErrorMessage = "Enter number between 0 to 99999999")]
        public decimal DeathCoverAmount { get; set; }

        [Display(Name = "Rating ID")]        
        [Required(AllowEmptyStrings = false, ErrorMessage = "RatingID should not be blank")]
        [Range(1, Int64.MaxValue, ErrorMessage = "Value should be greater than 0")]
        public int RatingID { get; set; }

        [Display(Name = "Age")]        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Age should not be blank")]
        [Range(18, 99, ErrorMessage = "Enter number between 18 to 99")]
        public int Age { get; set; }
    }
}
