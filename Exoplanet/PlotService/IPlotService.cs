using Newtonsoft.Json.Linq;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExoPlanet.NET.Exoplanet.PlotService
{
    /// <summary>
    /// Contains methods for plotting exoplanet data.
    /// </summary>
    public interface IPlotService
    {
        public Task<PlotModel> CreateComparisonGraph(string graphName, float xValue, float yValue, string xAxisTitle, string yAxisTitle, (float min, float max)? shadedBandY = null, string? shadedBandTitle = null);
    }
}
