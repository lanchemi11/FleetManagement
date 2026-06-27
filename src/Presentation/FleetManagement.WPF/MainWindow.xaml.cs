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
    public MainWindow(
        VehiclesViewModel vehiclesViewModel,
        DriversViewModel driversViewModel,
        TripsViewModel tripsViewModel,
        NotificationsViewModel notificationsViewModel,
        MaintenanceViewModel maintenanceViewModel)
    {
        InitializeComponent();

        VehiclesControl.DataContext = vehiclesViewModel;
        DriversControl.DataContext = driversViewModel;
        TripsControl.DataContext = tripsViewModel;
        NotificationsControl.DataContext = notificationsViewModel;
        MaintenanceControl.DataContext = maintenanceViewModel;
    }
}