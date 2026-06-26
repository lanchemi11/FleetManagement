using FleetManagement.SharedKernel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FleetManagement.Modules.Drivers.Domain
{
    public class Driver : Entity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string LicenseNumber { get; private set; }
        public DriverStatus Status { get; private set; }

        [JsonConstructor]
        public Driver(Guid id, string firstName, string lastName, string licenseNumber, DriverStatus status)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            LicenseNumber = licenseNumber;
            Status = status;
        }

        public Driver(string firstName, string lastName, string licenseNumber)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("Ime vozača ne može biti prazno.", nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Prezime vozača ne može biti prazno.", nameof(lastName));

            if (string.IsNullOrWhiteSpace(licenseNumber))
                throw new ArgumentException("Broj vozačke dozvole ne može biti prazan.", nameof(licenseNumber));

            FirstName = firstName;
            LastName = lastName;
            LicenseNumber = licenseNumber;
            Status = DriverStatus.Available;
        }

        public void AssignToTrip()
        {
            if (Status != DriverStatus.Available)
            {
                throw new InvalidOperationException("Vozač ne može biti angažovan jer trenutno ima aktivnu vožnju.");
            }
            Status = DriverStatus.Active;
        }

        public void Release()
        {
            if (Status != DriverStatus.Active)
            {
                throw new InvalidOperationException("Vozač već ima status slobodnog vozača.");
            }
            Status = DriverStatus.Available;
        }
    }
}
