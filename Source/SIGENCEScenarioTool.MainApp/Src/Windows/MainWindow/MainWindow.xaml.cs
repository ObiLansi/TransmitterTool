﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

using SIGENCEScenarioTool.Models.Database.GeoDb;
using SIGENCEScenarioTool.Tools;
using SIGENCEScenarioTool.ViewModels;



namespace SIGENCEScenarioTool.Windows.MainWindow
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            //-----------------------------------------------------------------

            if (string.IsNullOrEmpty(settings.UDPHost))
            {
                MB.Warning("The value in the configuration file for the setting UDPHost is invalid!\nPlease correct the value and restart the application.");
                settings.UDPHost = "127.0.0.1";
            }

            if (settings.UDPPortSending < 1025 || settings.UDPPortSending > 65535)
            {
                MB.Warning("The value in the configuration file for the setting UDPPort is invalid!\nPlease correct the value and restart the application.");
                settings.UDPPortSending = 4242;
            }

            if (settings.UDPDelay < 0 || settings.UDPDelay > 10000)
            {
                MB.Warning("The value in the configuration file for the setting UDPDelay is invalid!\nPlease correct the value and restart the application.");
                settings.UDPDelay = 500;
            }

            if (settings.MapZoomLevel < 1 || settings.MapZoomLevel > 20)
            {
                MB.Warning("The value in the configuration file for the setting MapZoomLevel is invalid!\nPlease correct the value and restart the application.");
                settings.MapZoomLevel = 18;
            }

            //-----------------------------------------------------------------

            this.RFDevicesCollection = new ObservableCollection<RFDeviceViewModel>();
            this.DataContext = this;

            //-----------------------------------------------------------------

            InitMapControl();
            InitMapProvider();
            InitCommands();
            InitFileOpenSaveDialogs();

            //-----------------------------------------------------------------

            SetTitle();
            UpdateScenarioDescription();

            //-----------------------------------------------------------------

#if DEBUG
            CreateRandomizedRFDevices(10);

            //OpenFile(@"D:\BigData\GitHub\SIGENCE-Scenario-Tool\Examples\TestScenario.stf");

            try
            {
                this.GeoNodeCollection = GeoNodeCollection.GetCollection(@"D:\BigData\GitHub\SIGENCE-Scenario-Tool\Databases\GeoDb\freiburg-regbez-latest.osm.sqlite");
                //this.GeoNodeCollection = GeoNodeCollection.GetCollection( @"C:\Lanser\Entwicklung\GitRepositories\SIGENCE-Scenario-Tool\Databases\GeoDb\freiburg-regbez-latest.osm.sqlite" );

                //-----------------------------------------------------------------

                //ListCollectionView _listCollectionView = CollectionViewSource.GetDefaultView(ocServices) as ListCollectionView;

                //if (_listCollectionView != null)
                //{
                //    _listCollectionView.IsLiveSorting = true;
                //    _listCollectionView.CustomSort = new ServiceComparer();
                //}

                //-----------------------------------------------------------------
            }
            catch (Exception ex)
            {
                MB.Error(ex);
            }
#endif
        }

    } // end public partial class MainWindow
}