using ParticleConverter.util;
using System.Windows;
using System.Windows.Threading;

namespace ParticleConverter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
        }

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString());
            Logger.WriteExceptionLog(e.Exception);
        }
    }
}
