using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ExoPlanet.NET.Exoplanet.Data
{
    /// <summary>
    /// Exoplanet properties class representing the properties of an exoplanet.
    /// </summary>
    public class ExoplanetProperties
    {
        [JsonProperty("pl_name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("pl_rade")]
        public float? Radius { get; set; } = 0.0f;

        [JsonProperty("pl_bmasse")]
        public float? Mass { get; set; } = 0.0f;

        [JsonProperty("pl_eqt")]
        public float? EquilibriumTemperature { get; set; } = 0.0f;

        [JsonProperty("st_teff")]
        public float? StarEffectiveTemperatureKelvin { get; set; } = 0.0f;

        [JsonProperty("st_lum")]
        public float? StarLuminosityLogSolar { get; set; } = 0.0f;

        [JsonProperty("pl_orbper")]
        public float? OrbitalPeriodDays { get; set; } = 0.0f;

        [JsonProperty("pl_orbsmax")]
        public float? SemiMajorAxisAU { get; set; } = 0.0f;
    }
}
