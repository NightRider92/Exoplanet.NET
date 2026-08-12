using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Threading.Tasks;

namespace ExoPlanet.NET.Exoplanet.PlotService
{
    /// <summary>
    /// Contains methods for plotting exoplanet data.
    /// </summary>
    public class PlotService : IPlotService
    {
        /// <summary>
        /// Creates a comparison graph with the specified parameters.
        /// </summary>
        /// <param name="graphName"></param>
        /// <param name="xValue"></param>
        /// <param name="yValue"></param>
        /// <param name="xAxisTitle"></param>
        /// <param name="yAxisTitle"></param>
        /// <param name="shadedBandY"></param>
        /// <param name="shadedBandTitle"></param>
        /// <returns></returns>
        public Task<PlotModel> CreateComparisonGraph(
            string graphName,
            float xValue,
            float yValue,
            string xAxisTitle,
            string yAxisTitle,
            (float min, float max)? shadedBandY = null,
            string? shadedBandTitle = null)
        {
            var model = new PlotModel
            {
                Title = graphName,
                TextColor = OxyColors.White,
                TitleFontWeight = FontWeights.Normal,
                TitleFontSize = 14
            };

            float maxX = xValue > 0 ? xValue * 2.0f : 1.0f;

            // Minimal visibility in case of very small values (e.g., AU scale)
            if (xAxisTitle.Contains("AU") && maxX < 0.1f)
            {
                maxX = Math.Max(maxX, 0.06f);
            }

            float maxY = yValue * 1.3f;

            if (shadedBandY.HasValue)
            {
                maxY = Math.Max(maxY, shadedBandY.Value.max * 1.2f);
            }

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = xAxisTitle,
                TextColor = OxyColors.White,
                Minimum = 0,
                Maximum = maxX,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                MajorGridlineColor = OxyColors.LightBlue,
                MinorGridlineColor = OxyColors.LightBlue,
            });

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = yAxisTitle,
                TextColor = OxyColors.White,
                Minimum = 0,
                Maximum = maxY,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                MajorGridlineColor = OxyColors.LightBlue,
                MinorGridlineColor = OxyColors.LightBlue
            });

            if (shadedBandY != null)
            {
                var area = new AreaSeries
                {
                    Color = OxyColor.FromAColor(80, OxyColors.LightGreen),
                    Title = shadedBandTitle ?? "Shaded zone"
                };

                area.Points.Add(new DataPoint(0, shadedBandY.Value.min));
                area.Points.Add(new DataPoint(maxX, shadedBandY.Value.min));

                area.Points2.Add(new DataPoint(0, shadedBandY.Value.max));
                area.Points2.Add(new DataPoint(maxX, shadedBandY.Value.max));

                model.Series.Add(area);
            }

            var scatter = new ScatterSeries
            {
                Title = "Target Planet",
                MarkerType = MarkerType.Circle,
                MarkerSize = 8,
                MarkerFill = OxyColors.DeepSkyBlue,
                MarkerStroke = OxyColors.White,
                MarkerStrokeThickness = 1.5
            };

            scatter.Points.Add(new ScatterPoint(xValue, yValue));
            model.Series.Add(scatter);

            return Task.FromResult(model);
        }
    }
}
