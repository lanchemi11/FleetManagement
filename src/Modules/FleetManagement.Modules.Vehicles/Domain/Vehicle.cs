using FleetManagement.SharedKernel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Modules.Vehicles.Domain
{
    public class Vehicle : Entity
    {
        public string Make { get; private set; }
        public string Model { get; private set; }
        public string PlateNumber { get; private set; }
        public int CurrentMileage { get; private set; }
        public VehicleStatus Status { get; private set; }

        public Vehicle(string make, string model, string plateNumber, int initialMileage)
        {
            if (string.IsNullOrWhiteSpace(make))
                throw new ArgumentException("Marka vozila ne može biti prazna.", nameof(make));

            if (string.IsNullOrWhiteSpace(model))
                throw new ArgumentException("Model vozila ne može biti prazan.", nameof(model));

            if (string.IsNullOrWhiteSpace(plateNumber))
                throw new ArgumentException("Registarska oznaka ne može biti prazna.", nameof(plateNumber));

            if (initialMileage < 0)
                throw new ArgumentException("Početna kilometraža ne može biti negativna.", nameof(initialMileage));

            Make = make;
            Model = model;
            PlateNumber = plateNumber;
            CurrentMileage = initialMileage;
            Status = VehicleStatus.Available;
        }

        public void UpdateMileage(int newMileage)
        {
            if (newMileage < CurrentMileage)
            {
                throw new InvalidOperationException("Nova kilometraža ne može biti manja od trenutne.");
            }
            CurrentMileage = newMileage;
        }

        public void AssignToTrip()
        {
            if (Status != VehicleStatus.Available)
            {
                throw new InvalidOperationException($"Vozilo ne može biti dodeljeno vožnji jer mu je status: {Status}.");
            }
            Status = VehicleStatus.InTrip;
        }

        public void CompleteTrip(int finalMileage)
        {
            if (Status != VehicleStatus.InTrip)
            {
                throw new InvalidOperationException("Vozilo nije na vožnji da bi se ona završila.");
            }
            UpdateMileage(finalMileage);
            Status = VehicleStatus.Available;
        }

        public void SendToMaintenance()
        {
            if (Status != VehicleStatus.Available)
            {
                throw new InvalidOperationException("Samo slobodno vozilo može biti poslato na servis.");
            }
            Status = VehicleStatus.InMaintenance;
        }

        public void ReleaseFromMaintenance()
        {
            if (Status != VehicleStatus.InMaintenance)
            {
                throw new InvalidOperationException("Vozilo se ne nalazi na servisu.");
            }
            Status = VehicleStatus.Available;
        }
    }
}
