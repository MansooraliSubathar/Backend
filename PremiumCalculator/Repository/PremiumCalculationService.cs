using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;
using PremiumCalculator.Common;
using PremiumCalculator.Model;
using System.IO;

namespace PremiumCalculator.Repository
{
    public class PremiumCalculationService : IPremiumCalculator
    {
        public readonly ILogger<PremiumCalculationService> _logger;
        public readonly IConfiguration _config;
        public readonly Utility _utility;
        public PremiumCalculationService(IConfiguration configuration, Utility utility, ILogger<PremiumCalculationService> logger)
        {
            _logger = logger;
            _config = configuration;
            _utility = utility;
        }
        public List<Occupation> GetOccupationList() // Retrieve all the Occupation list from source 
        {
            string relativePath = string.Empty;
            try
            {
                relativePath = _config.GetValue<string>("DataFilePath:Occupation");
                JArray OccupationJson = _utility.GetJSON(relativePath);
                if (OccupationJson != null)
                {
                    List<Occupation> OccupationList = (from res in OccupationJson
                                                       select new Occupation
                                                       {
                                                           ID = (int)res["id"],
                                                           Name = res["name"].ToString(),
                                                           RatingID = (int)res["rating_id"]
                                                       }).ToList();
                    return OccupationList;
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
            }
            finally
            {
                relativePath = string.Empty;
            }
            return null;
        }
        public decimal CalculatePremium(UserData userData) //calculate the premium as per the given formula
        {
            try
            {
                var RatingFactorDetails = GetRatingFactorDetails(userData.RatingID);
                if (RatingFactorDetails == null)
                    return 0;

                decimal DeathPremium = (userData.DeathCoverAmount * RatingFactorDetails.FactorValue * userData.Age) / 1000 * 12;
                return DeathPremium;
            }
            catch (System.Exception ex) { _logger.LogError(ex.Message, ex.StackTrace); }

            return 0;
        }
                
        public Rating GetRatingFactorDetails(int ratingId)
        {
            string relativePath = string.Empty;
            try
            {
                relativePath = _config.GetValue<string>("DataFilePath:Rating");
                JArray RatingJson = _utility.GetJSON(relativePath);
                if (RatingJson != null)
                {
                    Rating RatingDetails = (from res in RatingJson
                                            where (int)res["id"] == ratingId
                                            select new Rating
                                            {
                                                ID = (int)res["id"],
                                                Name = res["name"].ToString(),
                                                FactorValue = (decimal)res["factor"]
                                            }).FirstOrDefault();
                    return RatingDetails;
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
            }
            finally
            {
                relativePath = string.Empty;
            }
            return null;
        }
    }
}
