using ExoPlanet.NET.Exoplanet;
using ExoPlanet.NET.Exoplanet.Data;
using ExoPlanet.NET.Exoplanet.PlotService;
using ExoPlanet.NET.Utilities;
using Newtonsoft.Json.Linq;
using OxyPlot;
using OxyPlot.Series;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExoPlanet.NET
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainViewModel ViewModel = new MainViewModel();
        private readonly IPlotService _plotService;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = ViewModel;
            this._plotService = new PlotService();
        }

        /// <summary>
        /// On grid loaded event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.loadingBarControl1.Visibility = Visibility.Visible;
                this.errorPanel.Visibility = Visibility.Hidden;

                IExoplanet exoplanet = new Exoplanet.Exoplanet();
                var planets = await exoplanet.GetDataAsync();

                if (planets is null) throw new ArgumentNullException(nameof(planets));
                foreach (var p in planets)
                {
                    if (string.IsNullOrEmpty(p.Name)) continue;
                    p.Name = (p.Name.Length < 48 ? p.Name : $"{p.Name.Substring(0, 48)} ...").Trim();
                    this.ViewModel.Exoplanets?.Add(p);
                }

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                this.errorPanel.Visibility = Visibility.Visible;
            }
            finally
            {
                this.loadingBarControl1.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Listbox selection changed event to update the plot based on the selected exoplanet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var planet = ViewModel.SelectedExoplanet;
            if (planet == null) return;

            float? radius = planet.Radius;
            float? semiMajorAxis = planet.SemiMajorAxisAU;
            float? equilibriumTemperature = planet.EquilibriumTemperature;

            // Update graphs
            plotViewControl.Model = await _plotService.CreateComparisonGraph(
                "Radius–Temperature Relation",
                xValue: radius ?? 0f,
                yValue: equilibriumTemperature ?? 0f,
                xAxisTitle: "Radius (Earth radii)",
                yAxisTitle: "Equilibrium Temperature (K)",
                shadedBandY: (240, 310),
                shadedBandTitle: "Habitable Zone");

            plotViewControl2.Model = await _plotService.CreateComparisonGraph(
                "Equilibrium Temperature as a Function of Orbital Distance",
                xValue: semiMajorAxis ?? 0f,
                yValue: equilibriumTemperature ?? 0f,
                xAxisTitle: "Semi-Major Axis (AU)",
                yAxisTitle: "Equilibrium Temperature (K)",
                shadedBandY: (240, 310),
                shadedBandTitle: "Habitable Zone");

            // Habitable zone and world type classification
            if (radius.HasValue && equilibriumTemperature.HasValue)
            {
                bool isHabitable = WorldClassification.IsHabitableCandidate(radius.Value, equilibriumTemperature.Value);
                HabitableZoneLabel.Content = $"◯ Habitable zone: {(isHabitable ? "Yes" : "No")}";

                string classification = WorldClassification.Classify(radius.Value, equilibriumTemperature.Value);
                worldTypeLabel.Content = $"◯ World type classification: {classification}";
            }
            else
            {
                HabitableZoneLabel.Content = "◯ Habitable zone: N/A";
                worldTypeLabel.Content = "◯ World type classification: N/A";
            }
        }

        /// <summary>
        /// Text box filtering
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var view = CollectionViewSource.GetDefaultView(ViewModel.Exoplanets);
            if (view == null) return;

            view.Filter = item =>
            {
                if (item is ExoplanetProperties p)
                {
                    if (string.IsNullOrWhiteSpace(searchBox.Text))
                        return true;

                    return p.Name.Contains(searchBox.Text, StringComparison.OrdinalIgnoreCase);
                }
                return false;
            };
            view.Refresh();
        }
    }
}