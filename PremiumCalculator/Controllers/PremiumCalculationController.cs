using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using PremiumCalculator.Model;
using PremiumCalculator.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PremiumCalculator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PremiumCalculationController : ControllerBase
    {
        private readonly IPremiumCalculator _premiumCalculatorService;
        //private readonly ILogger _logger;
        public PremiumCalculationController(IPremiumCalculator premiumCalculator)
        {
            this._premiumCalculatorService = premiumCalculator;
            //this._logger = logger;
        }

        [HttpGet("GetOccupationList")]
        //[OutputCache(10)]
        public ActionResult GetOccupationList()
        {
            List<Occupation> OccupationList =  this._premiumCalculatorService.GetOccupationList();
            if (OccupationList == null)
                return NotFound();

            return Ok(OccupationList);
        }

        
        [HttpPost("CalculatePremium")]
        public ActionResult CalculatePremium([FromBody]UserData userData)
        {
            if (userData == null)
                return NotFound();

            decimal mothlyPremium = this._premiumCalculatorService.CalculatePremium(userData);            

            return Ok(mothlyPremium);
        }

       
    }
}
