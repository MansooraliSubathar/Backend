using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PremiumCalculator.Model
{
    public class UserData
    {        
        public double DeathCoverAmount { get; set; }
        public int RatingID { get; set; }
        public int Age { get; set; }
    }
}
