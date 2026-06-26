using FleetManagement.Modules.Drivers.Application;
using FleetManagement.Modules.Drivers.Domain;
using FleetManagement.Modules.Trips.Application;
using FleetManagement.Modules.Vehicles.Application;
using FleetManagement.Modules.Vehicles.Domain;
using FleetManagement.WPF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FleetManagement.WPF.ViewModels
{
    public record TripDisplayModel(
    Guid Id,
    string VehicleDetails,
    string DriverName,
    string StartLocation,
    string EndLocation,
    int StartMileage,
    int? EndMileage,
    string Status
);

    public class TripsViewModel : ViewModelBase
    {
        private readonly StartTripUseCase _startTripUseCase;
        private readonly CompleteTripUseCase _completeTripUseCase;
        private readonly ITripRepository _tripRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IDriverRepository _driverRepository;

        public ObservableCollection<Vehicle> AvailableVehicles { get; } = new();
        public ObservableCollection<Driver> AvailableDrivers { get; } = new();

        public ObservableCollection<TripDisplayModel> TripsList { get; } = new();

        private Vehicle? _selectedVehicle;
        private Driver? _selectedDriver;
        private string _startLocation = string.Empty;
        private string _endLocation = string.Empty;

        public Vehicle? SelectedVehicle { get => _selectedVehicle; set => SetProperty(ref _selectedVehicle, value); }
        public Driver? _SelectedDriver { get => _selectedDriver; set => SetProperty(ref _selectedDriver, value); }
        public string StartLocation { get => _startLocation; set => SetProperty(ref _startLocation, value); }
        public string EndLocation { get => _endLocation; set => SetProperty(ref _endLocation, value); }

        private TripDisplayModel? _selectedTrip;
        private int _endMileage;

        public TripDisplayModel? SelectedTrip { get => _selectedTrip; set => SetProperty(ref _selectedTrip, value); }
        public int EndMileage { get => _endMileage; set => SetProperty(ref _endMileage, value); }

        public ICommand StartTripCommand { get; }
        public ICommand CompleteTripCommand { get; }

        public TripsViewModel(
            StartTripUseCase startTripUseCase,
            CompleteTripUseCase completeTripUseCase,
            ITripRepository tripRepository,
            IVehicleRepository vehicleRepository,
            IDriverRepository driverRepository)
        {
            _startTripUseCase = startTripUseCase;
            _completeTripUseCase = completeTripUseCase;
            _tripRepository = tripRepository;
            _vehicleRepository = vehicleRepository;
            _driverRepository = driverRepository;

            StartTripCommand = new RelayCommand(async () => await StartTripAsync());
            CompleteTripCommand = new RelayCommand(async () => await CompleteTripAsync());

            _ = LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            try
            {
                var vehicles = await _vehicleRepository.GetAllAsync();
                AvailableVehicles.Clear();
                foreach (var v in vehicles.Where(x => x.Status == VehicleStatus.Available))
                {
                    AvailableVehicles.Add(v);
                }

                var drivers = await _driverRepository.GetAllAsync();
                AvailableDrivers.Clear();
                foreach (var d in drivers.Where(x => x.Status == DriverStatus.Available))
                {
                    AvailableDrivers.Add(d);
                }

                var trips = await _tripRepository.GetAllAsync();
                TripsList.Clear();
                foreach (var t in trips)
                {
                    var vehicle = await _vehicleRepository.GetByIdAsync(t.VehicleId);
                    var driver = await _driverRepository.GetByIdAsync(t.DriverId);

                    string vehicleDetails = vehicle != null ? $"{vehicle.Make} {vehicle.Model} ({vehicle.PlateNumber})" : "Nepoznato vozilo";
                    string driverName = driver != null ? $"{driver.FirstName} {driver.LastName}" : "Nepoznat vozač";
                    string status = t.IsCompleted ? "Završeno" : "Aktivno";

                    TripsList.Add(new TripDisplayModel(
                        t.Id,
                        vehicleDetails,
                        driverName,
                        t.StartLocation,
                        t.EndLocation,
                        t.StartMileage,
                        t.EndMileage,
                        status
                    ));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri osvežavanju podataka: {ex.Message}");
            }
        }

        private async Task StartTripAsync()
        {
            if (SelectedVehicle == null || _SelectedDriver == null)
            {
                MessageBox.Show("Molimo izaberite slobodno vozilo i vozača.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var command = new StartTripCommand(SelectedVehicle.Id, _SelectedDriver.Id, StartLocation, EndLocation);
                await _startTripUseCase.ExecuteAsync(command);

                MessageBox.Show("Vožnja je uspešno pokrenuta! Vozilo i vozač su sada zauzeti.");

                SelectedVehicle = null;
                _SelectedDriver = null;
                StartLocation = string.Empty;
                EndLocation = string.Empty;

                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task CompleteTripAsync()
        {
            if (SelectedTrip == null)
            {
                MessageBox.Show("Molimo izaberite aktivnu vožnju iz tabele.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SelectedTrip.Status == "Završeno")
            {
                MessageBox.Show("Izabrana vožnja je već završena.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var command = new CompleteTripCommand(SelectedTrip.Id, EndMileage);
                await _completeTripUseCase.ExecuteAsync(command);

                MessageBox.Show("Vožnja je uspešno završena! Vozilo i vozač su ponovo slobodni.");

                SelectedTrip = null;
                EndMileage = 0;

                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
