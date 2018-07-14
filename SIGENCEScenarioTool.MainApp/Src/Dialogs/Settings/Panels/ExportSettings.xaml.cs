﻿using System.Windows.Controls;



namespace SIGENCEScenarioTool.Dialogs.Settings.Panels
{
    /// <summary>
    /// Interaktionslogik für ExportSettings.xaml
    /// </summary>
    public partial class ExportSettings : UserControl, ISettingsControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportSettings"/> class.
        /// </summary>
        public ExportSettings()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Gets the Image.
        /// </summary>
        /// <returns></returns>
        public Image GetImage()
        {
            return (Image)Resources["EXPORT"];
        }


        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return "Export Settings";
        }

    } // end public partial class ExportSettings
}
