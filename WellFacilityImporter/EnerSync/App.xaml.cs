using CommunityToolkit.Mvvm.DependencyInjection;
using EnerSync.Data;
using EnerSync.Services;
using EnerSync.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace EnerSync
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider? ServiceProvider { get; private set; }

        /// <summary>
        /// Handles the startup event of the application.
        /// </summary>
        /// <param name="e">The <see cref="StartupEventArgs"/> instance containing the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
            Ioc.Default.ConfigureServices(ServiceProvider);  // Set up Ioc

            base.OnStartup(e);
        }

        /// <summary>
        /// Configures the services for dependency injection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<EnerSyncContext>();

            // Register the IDataService and its implementation
            services.AddSingleton<IDataService, DataService>();

            // Register the FilterService
            services.AddTransient<FilterService>();

            // Register the IDialogService and its implementation
            services.AddSingleton<IDialogService, DialogService>();

            // Register the MainViewModel
            services.AddSingleton<MainViewModel>();

            // Register the WellsFilterViewModel
            services.AddSingleton<WellsFilterViewModel>();

            // Register the FacilitiesFilterViewModel
            services.AddSingleton<FacilitiesFilterViewModel>();

            // Register the WellWikiImporterViewModel
            services.AddSingleton<WellWikiImporterViewModel>();
        }
    }

}
