using FleetManagement.Modules.Maintenance.Application;
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
    public record MaintenanceRecordDisplayModel(
        Guid Id,
        string VehiclePlate,
        string Description,
        decimal Cost,
        DateTime ServiceDate
    );

    public class MaintenanceViewModel : ViewModelBase
    {
        private readonly RecordMaintenanceUseCase _recordMaintenanceUseCase;
        private readonly ReleaseFromMaintenanceUseCase _releaseFromMaintenanceUseCase;
        private readonly IMaintenanceRepository _maintenanceRepository;
        private readonly IVehicleRepository _vehicleRepository;

        public ObservableCollection<Vehicle> AvailableVehicles { get; } = new();
        public ObservableCollection<Vehicle> VehiclesInMaintenance { get; } = new();
        public ObservableCollection<MaintenanceRecordDisplayModel> MaintenanceList { get; } = new();

        private Vehicle? _selectedAvailableVehicle;
        private string _description = string.Empty;
        private decimal _cost;

        public Vehicle? SelectedAvailableVehicle { get => _selectedAvailableVehicle; set => SetProperty(ref _selectedAvailableVehicle, value); }
        public string Description { get => _description; set => SetProperty(ref _description, value); }
        public decimal Cost { get => _cost; set => SetProperty(ref _cost, value); }

        private Vehicle? _selectedVehicleToRelease;
        public Vehicle? SelectedVehicleToRelease { get => _selectedVehicleToRelease; set => SetProperty(ref _selectedVehicleToRelease, value); }

        public ICommand SendToMaintenanceCommand { get; }
        public ICommand ReleaseFromMaintenanceCommand { get; }

        public MaintenanceViewModel(
            RecordMaintenanceUseCase recordMaintenanceUseCase,
            ReleaseFromMaintenanceUseCase releaseFromMaintenanceUseCase,
            IMaintenanceRepository maintenanceRepository,
            IVehicleRepository vehicleRepository)
        {
            _recordMaintenanceUseCase = recordMaintenanceUseCase;
            _releaseFromMaintenanceUseCase = releaseFromMaintenanceUseCase;
            _maintenanceRepository = maintenanceRepository;
            _vehicleRepository = vehicleRepository;

            SendToMaintenanceCommand = new RelayCommand(async () => await SendToMaintenanceAsync());
            ReleaseFromMaintenanceCommand = new RelayCommand(async () => await ReleaseFromMaintenanceAsync());

            _ = LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            try
            {
                var vehicles = await _vehicleRepository.GetAllAsync();
                var records = await _maintenanceRepository.GetAllAsync();

                var availableList = vehicles.Where(v => v.Status == VehicleStatus.Available).ToList();
                var inMaintenanceList = vehicles.Where(v => v.Status == VehicleStatus.InMaintenance).ToList();

                var displayRecords = new System.Collections.Generic.List<MaintenanceRecordDisplayModel>();
                foreach (var r in records)
                {
                    var vehicle = await _vehicleRepository.GetByIdAsync(r.VehicleId);
                    string vehiclePlate = vehicle != null ? $"{vehicle.Make} {vehicle.Model} ({vehicle.PlateNumber})" : "Nepoznato vozilo";
                    displayRecords.Add(new MaintenanceRecordDisplayModel(r.Id, vehiclePlate, r.Description, r.Cost, r.ServiceDate));
                }

                if (Application.Current?.Dispatcher != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        AvailableVehicles.Clear();
                        foreach (var v in availableList) AvailableVehicles.Add(v);

                        VehiclesInMaintenance.Clear();
                        foreach (var v in inMaintenanceList) VehiclesInMaintenance.Add(v);

                        MaintenanceList.Clear();
                        foreach (var r in displayRecords.OrderByDescending(x => x.ServiceDate)) MaintenanceList.Add(r);
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri učitavanju podataka o održavanju: {ex.Message}");
            }
        }

        private async Task SendToMaintenanceAsync()
        {
            if (SelectedAvailableVehicle == null)
            {
                MessageBox.Show("Molimo izaberite slobodno vozilo.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var command = new RecordMaintenanceCommand(SelectedAvailableVehicle.Id, Description, Cost);
                await _recordMaintenanceUseCase.ExecuteAsync(command);

                MessageBox.Show("Vozilo je uspešno poslato na servis! Više se ne može birati za vožnje.");

                Description = string.Empty;
                Cost = 0;
                SelectedAvailableVehicle = null;

                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task ReleaseFromMaintenanceAsync()
        {
            if (SelectedVehicleToRelease == null)
            {
                MessageBox.Show("Molimo izaberite vozilo koje je trenutno na servisu.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                await _releaseFromMaintenanceUseCase.ExecuteAsync(SelectedVehicleToRelease.Id);

                MessageBox.Show("Vozilo je uspešno vraćeno sa servisa i ponovo je slobodno za vožnje!");

                SelectedVehicleToRelease = null;

                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
