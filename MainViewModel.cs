using ExoPlanet.NET.Exoplanet.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace ExoPlanet.NET
{
    /// <summary>
    /// ViewModel for the main window of the application.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Collection of loaded exoplanets.
        /// ObservableCollection is required so WPF updates UI when items are added.
        /// </summary>
        public ObservableCollection<ExoplanetProperties?> Exoplanets { get; set; }
            = new ObservableCollection<ExoplanetProperties?>();

        private ExoplanetProperties? _selectedExoplanet = null;

        /// <summary>
        /// Currently selected exoplanet.
        /// Must raise OnPropertyChanged so UI updates planet details + graphs.
        /// </summary>
        public ExoplanetProperties? SelectedExoplanet
        {
            get => _selectedExoplanet;
            set
            {
                if (_selectedExoplanet != value)
                {
                    _selectedExoplanet = value;
                    OnPropertyChanged(nameof(SelectedExoplanet));
                }
            }
        }

        /// <summary>
        /// Helper method for notifying WPF bindings.
        /// </summary>
        /// <param name="name"></param>
        void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
