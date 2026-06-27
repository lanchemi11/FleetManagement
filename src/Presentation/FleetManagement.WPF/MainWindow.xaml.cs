using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FleetManagement.WPF.ViewModels;

namespace FleetManagement.WPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly VehiclesViewModel _vehiclesViewModel;
    private readonly DriversViewModel _driversViewModel;
    private readonly TripsViewModel _tripsViewModel;
    private readonly NotificationsViewModel _notificationsViewModel;
    private readonly MaintenanceViewModel _maintenanceViewModel;

    public MainWindow(
        VehiclesViewModel vehiclesViewModel,
        DriversViewModel driversViewModel,
        TripsViewModel tripsViewModel,
        NotificationsViewModel notificationsViewModel,
        MaintenanceViewModel maintenanceViewModel)
    {
        InitializeComponent();

        _vehiclesViewModel = vehiclesViewModel;
        _driversViewModel = driversViewModel;
        _tripsViewModel = tripsViewModel;
        _notificationsViewModel = notificationsViewModel;
        _maintenanceViewModel = maintenanceViewModel;

        VehiclesControl.DataContext = _vehiclesViewModel;
        DriversControl.DataContext = _driversViewModel;
        TripsControl.DataContext = _tripsViewModel;
        NotificationsControl.DataContext = _notificationsViewModel;
        MaintenanceControl.DataContext = _maintenanceViewModel;
    }

    private async void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.Source is TabControl)
        {
            switch (MainTabControl.SelectedIndex)
            {
                case 2:
                    await _tripsViewModel.LoadDataAsync();
                    break;
                case 3:
                    await _notificationsViewModel.LoadNotificationsAsync();
                    break;
                case 4:
                    await _maintenanceViewModel.LoadDataAsync();
                    break;
            }
        }
    }
}