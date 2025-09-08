using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;

namespace zoft.MauiExtensions.Sample.Views
{
    /// <summary>
    /// Simple about page for the sample application. Displays basic information about the library and developer.
    /// </summary>
    public partial class AboutView : ContentPage
    {
        public AboutView()
        {
            InitializeComponent();

            VersionLabel.Text = $"Version {AppInfo.VersionString} (Build {AppInfo.BuildString})";
        }

        /// <summary>
        /// Opens the GitHub repository when the corresponding label is tapped.
        /// </summary>
        private async void OnGitHubLinkTapped(object sender, EventArgs e)
        {
            try
            {
                await Launcher.Default.OpenAsync(new Uri("https://github.com/zleao/zoft.MauiExtensions"));
            }
            catch (Exception)
            {
                // optionally handle exceptions if the link cannot be opened
            }
        }

        /// <summary>
        /// Opens the default mail client with a prefilled email when the email label is tapped.
        /// </summary>
        private async void OnEmailLinkTapped(object sender, EventArgs e)
        {
            try
            {
                await Launcher.Default.OpenAsync(new Uri("mailto:zleaopereira@gmail.com"));
            }
            catch (Exception)
            {
                // optionally handle exceptions if the link cannot be opened
            }
        }
    }
}