using FleetManagement.Modules.Drivers.Application;
using FleetManagement.Modules.Drivers.Domain;
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
    public class DriversViewModel : ViewModelBase
    {
        private readonly RegisterDriverUseCase _registerDriverUseCase;
        private readonly IDriverRepository _driverRepository;

        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _licenseNumber = string.Empty;

        public string FirstName { get => _firstName; set => SetProperty(ref _firstName, value); }
        public string LastName { get => _lastName; set => SetProperty(ref _lastName, value); }
        public string LicenseNumber { get => _licenseNumber; set => SetProperty(ref _licenseNumber, value); }

        public ObservableCollection<Driver> DriversList { get; } = new();

        public ICommand RegisterDriverCommand { get; }

        public DriversViewModel(RegisterDriverUseCase registerDriverUseCase, IDriverRepository driverRepository)
        {
            _registerDriverUseCase = registerDriverUseCase;
            _driverRepository = driverRepository;

            RegisterDriverCommand = new RelayCommand(async () => await RegisterDriverAsync());

            _ = LoadDriversAsync();
        }

        private async Task LoadDriversAsync()
        {
            try
            {
                var drivers = await _driverRepository.GetAllAsync();
                DriversList.Clear();
                foreach (var driver in drivers)
                {
                    DriversList.Add(driver);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri učitavanju vozača: {ex.Message}");
            }
        }

        private async Task RegisterDriverAsync()
        {
            try
            {
                var command = new RegisterDriverCommand(FirstName, LastName, LicenseNumber);
                await _registerDriverUseCase.ExecuteAsync(command);

                MessageBox.Show("Vozač je uspešno registrovan!");

                FirstName = string.Empty;
                LastName = string.Empty;
                LicenseNumber = string.Empty;

                await LoadDriversAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
