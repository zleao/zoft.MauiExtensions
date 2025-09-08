namespace zoft.MauiExtensions.Sample
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            VersionLabel.Text = $"Version {AppInfo.VersionString} (Build {AppInfo.BuildString})";
        }
    }
}