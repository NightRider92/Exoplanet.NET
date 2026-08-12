namespace ExoPlanet.NET.Utilities
{
    public static class WorldClassification
    {
        /// <summary>
        /// World classification based on radius and temperature.
        /// </summary>
        public static bool IsHabitableCandidate(float radiusEarth, float tempKelvin)
        {
            bool isSolid = radiusEarth is > 0.3f and <= 1.6f;
            bool isTemperate = tempKelvin is >= 240f and <= 310f;

            return isSolid && isTemperate;
        }

        /// <summary>
        /// Exoplanet classification based on radius and temperature.
        /// </summary>
        public static string Classify(float radiusEarth, float tempKelvin)
        {
            string sizeType;
            if (radiusEarth < 0.5f) sizeType = "Sub-Earth / Mercurian";
            else if (radiusEarth < 0.8f) sizeType = "Sub-Earth / Sub-terran";
            else if (radiusEarth <= 1.25f) sizeType = "Terran (Earth-sized)";
            else if (radiusEarth <= 1.6f) sizeType = "Super-Earth (Rocky)";
            else if (radiusEarth <= 4.0f) sizeType = "Sub-Neptune / Mini-Neptune";
            else sizeType = "Gas Giant / Jovian";

            string tempType;
            if (tempKelvin < 200f) tempType = "Frozen";
            else if (tempKelvin < 240f) tempType = "Cold";
            else if (tempKelvin <= 310f) tempType = "Temperate (Habitable range)";
            else if (tempKelvin <= 450f) tempType = "Warm";
            else if (tempKelvin <= 1000f) tempType = "Hot";
            else tempType = "Ultra-hot";

            if (tempKelvin is >= 240f and <= 310f && radiusEarth > 1.6f)
            {
                return $"{tempType} {sizeType} (Gas giant - Gas Surface, Moons might be habitable)";
            }

            return $"{tempType} {sizeType}";
        }
    }
}
