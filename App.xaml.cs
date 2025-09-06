using Microsoft.UI.Xaml;

namespace VncRdpLauncher
{
    public partial class App : Application
    {
        public App()
        {
        }
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            var window = new MainWindow();
            window.Activate(); // This is the magic line — without it, nothing shows
        }
    }
}