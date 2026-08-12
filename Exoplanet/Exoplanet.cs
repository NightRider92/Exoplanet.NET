using ExoPlanet.NET.Exoplanet.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace ExoPlanet.NET.Exoplanet
{
    /// <summary>
    /// Represents an exoplanet and provides methods to retrieve its data.
    /// </summary>
    public class Exoplanet : IExoplanet
    {
        public const string URI = "https://exoplanetarchive.ipac.caltech.edu/TAP/sync?query=select+top+100000+pl_name,pl_rade,pl_bmasse,pl_eqt,st_teff,st_lum,pl_orbper,pl_orbsmax+from+pscomppars&format=json";
      
        /// <summary>
        /// Constructor
        /// </summary>
        public Exoplanet()
        {

        }

        /// <summary>
        /// Get exoplanet data asynchronously
        /// </summary>
        /// <returns></returns>
        public async Task<ConcurrentBag<ExoplanetProperties?>?> GetDataAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var result = await client.GetAsync(URI);
                    string jsonString = await result.Content.ReadAsStringAsync();

                    if (string.IsNullOrEmpty(jsonString)) throw new ArgumentNullException(nameof(jsonString));
                    return JsonConvert.DeserializeObject<ConcurrentBag<ExoplanetProperties?>?>(jsonString);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}
