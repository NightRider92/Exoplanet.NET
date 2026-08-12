using ExoPlanet.NET.Exoplanet.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ExoPlanet.NET.Exoplanet
{
    /// <summary>
    /// Interface for exoplanet data retrieval.
    /// </summary>
    public interface IExoplanet
    {
        public Task<ConcurrentBag<ExoplanetProperties?>?> GetDataAsync();
    }
}
