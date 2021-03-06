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
            JArray OccupationJson = null;
            try
            {
                relativePath = _config.GetValue<string>("DataFilePath:Occupation");
                OccupationJson = _utility.GetJSON(relativePath);
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
                OccupationJson = null;
                relativePath = string.Empty;
            }
            return null;
        }
        public decimal CalculatePremium(UserData userData) //calculate the premium as per the given formula
        {
            Rating RatingFactorDetails = null;
            try
            {
                RatingFactorDetails = GetRatingFactorDetails(userData.OccupationId);
                if (RatingFactorDetails == null)
                    return 0;

                decimal DeathPremium = (userData.DeathCoverAmount * RatingFactorDetails.FactorValue * userData.Age) / 1000 * 12;
                return DeathPremium;
            }
            catch (System.Exception ex) { _logger.LogError(ex.Message, ex.StackTrace); }
            finally { RatingFactorDetails = null; }

            return 0;
        }

        public Rating GetRatingFactorDetails(int occupationId)
        {
            string relativePath = string.Empty;
            JArray RatingJson = null;
            JArray OccupationJson = null;
            try
            {
                relativePath = _config.GetValue<string>("DataFilePath:Rating");
                RatingJson = _utility.GetJSON(relativePath);

                string OccupationRelativePath = _config.GetValue<string>("DataFilePath:Occupation");
                OccupationJson = _utility.GetJSON(OccupationRelativePath);

                if (RatingJson != null && OccupationJson!=null)
                {
                    Rating RatingDetails = (from res in RatingJson
                                            join occu in OccupationJson on (int)res["id"] equals ((int)occu["rating_id"])
                                            where (int)occu["id"] == occupationId
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
                RatingJson = null;
                OccupationJson = null;
                relativePath = string.Empty;
            }
            return null;
        }
    }
}
