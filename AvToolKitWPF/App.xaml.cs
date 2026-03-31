using System;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using AvToolKitWPF.Login_Create;

namespace AvToolKitWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }
        private void ConfigureServices(IServiceCollection services)
        {
  

            services.AddTransient<Login>();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            try
            {
                var loginWindow = ServiceProvider.GetRequiredService<Login>();

                loginWindow.Show();
            }
            catch (InvalidOperationException ioex)
            {
                MessageBox.Show($" Error : {ioex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}
