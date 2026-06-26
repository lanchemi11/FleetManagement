using FleetManagement.Modules.Drivers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Modules.Drivers.Application
{
    public class RegisterDriverUseCase
    {
        private readonly IDriverRepository _driverRepository;

        public RegisterDriverUseCase(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public async Task ExecuteAsync(RegisterDriverCommand command)
        {
            var existingDriver = await _driverRepository.GetByLicenseNumberAsync(command.LicenseNumber);
            if (existingDriver != null)
            {
                throw new InvalidOperationException($"Vozač sa brojem dozvole '{command.LicenseNumber}' već postoji u sistemu.");
            }

            var driver = new Driver(
                command.FirstName,
                command.LastName,
                command.LicenseNumber
            );

            await _driverRepository.AddAsync(driver);
        }
    }
}
