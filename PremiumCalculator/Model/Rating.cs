using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PremiumCalculator.Model
{
    public class Rating
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal FactorValue { get; set; }
    }
}
