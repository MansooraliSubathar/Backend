using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PremiumCalculator.Common
{
    //Common Utility = grouped all the common logic to share across services
    public class Utility 
    {
        public JArray GetJSON(string relativePath)
        {
            string basePath = Directory.GetCurrentDirectory();
            string filePath = Path.GetFullPath(basePath + relativePath);
            if (File.Exists(filePath))
            {
                var list = File.ReadAllText(filePath);
                return JArray.Parse(list);
            }

            return null;
        }

        
    }
}
