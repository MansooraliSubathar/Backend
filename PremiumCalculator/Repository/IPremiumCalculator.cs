using Newtonsoft.Json.Linq;
using PremiumCalculator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PremiumCalculator.Repository
{
    // Implement this interface to do the Premium calculate logic across the types of data sources (like json, sql, oracle)
    public interface IPremiumCalculator
    {
        public List<Occupation> GetOccupationList();
        public Rating GetRatingFactorDetails(int ratingId);
        public double CalculatePremium(UserData userData);
    }
}
