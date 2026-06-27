using FleetManagement.Modules.Drivers.Application;
using FleetManagement.Modules.Drivers.Infrastructure;
using FleetManagement.Modules.Maintenance.Application;
using FleetManagement.Modules.Maintenance.Infrastructure;
using FleetManagement.Modules.Notifications.Application;
using FleetManagement.Modules.Notifications.Infrastructure;
using FleetManagement.Modules.Trips.Application;
using FleetManagement.Modules.Trips.Infrastructure;
using FleetManagement.Modules.Vehicles.Application;
using FleetManagement.Modules.Vehicles.Infrastructure;
using FleetManagement.SharedKernel.Events;
using FleetManagement.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace FleetManagement.WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    public App()
    {
        var services = new ServiceCollection();

        ConfigureServices(services);

        _serviceProvider = services.BuildServiceProvider();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IEventBus, InMemoryEventBus>();

        services.AddSingleton<IVehicleRepository, JsonVehicleRepository>();
        services.AddTransient<RegisterVehicleUseCase>();
        services.AddTransient<VehiclesViewModel>();

        services.AddSingleton<IDriverRepository, JsonDriverRepository>();
        services.AddTransient<RegisterDriverUseCase>();
        services.AddTransient<DriversViewModel>();

        services.AddSingleton<ITripRepository, JsonTripRepository>();
        services.AddTransient<StartTripUseCase>();
        services.AddTransient<CompleteTripUseCase>();
        services.AddTransient<TripsViewModel>();

        services.AddSingleton<INotificationRepository, JsonNotificationRepository>();
        services.AddTransient<NotificationsViewModel>();
        services.AddTransient<IEventHandler<TripCompletedEvent>, TripCompletedEventHandler>();

        services.AddSingleton<IMaintenanceRepository, JsonMaintenanceRepository>();
        services.AddTransient<RecordMaintenanceUseCase>();
        services.AddTransient<ReleaseFromMaintenanceUseCase>();
        services.AddTransient<MaintenanceViewModel>();

        services.AddTransient<MainWindow>();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }
}

