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
    public class VehiclesViewModel : ViewModelBase
    {
        private readonly RegisterVehicleUseCase _registerVehicleUseCase;
        private readonly IVehicleRepository _vehicleRepository;

        private string _make = string.Empty;
        private string _model = string.Empty;
        private string _plateNumber = string.Empty;
        private int _initialMileage;

        public string Make { get => _make; set => SetProperty(ref _make, value); }
        public string Model { get => _model; set => SetProperty(ref _model, value); }
        public string PlateNumber { get => _plateNumber; set => SetProperty(ref _plateNumber, value); }
        public int InitialMileage { get => _initialMileage; set => SetProperty(ref _initialMileage, value); }

        public ObservableCollection<Vehicle> VehiclesList { get; } = new();

        public ICommand RegisterVehicleCommand { get; }

        public VehiclesViewModel(RegisterVehicleUseCase registerVehicleUseCase, IVehicleRepository vehicleRepository)
        {
            _registerVehicleUseCase = registerVehicleUseCase;
            _vehicleRepository = vehicleRepository;

            RegisterVehicleCommand = new RelayCommand(async () => await RegisterVehicleAsync());

            _ = LoadVehiclesAsync();
        }

        private async Task LoadVehiclesAsync()
        {
            try
            {
                var vehicles = await _vehicleRepository.GetAllAsync();
                VehiclesList.Clear();
                foreach (var vehicle in vehicles)
                {
                    VehiclesList.Add(vehicle);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri učitavanju vozila: {ex.Message}");
            }
        }

        private async Task RegisterVehicleAsync()
        {
            try
            {
                var command = new RegisterVehicleCommand(Make, Model, PlateNumber, InitialMileage);
                await _registerVehicleUseCase.ExecuteAsync(command);

                MessageBox.Show("Vozilo je uspešno registrovano!");

                Make = string.Empty;
                Model = string.Empty;
                PlateNumber = string.Empty;
                InitialMileage = 0;

                await LoadVehiclesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri registraciji: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
