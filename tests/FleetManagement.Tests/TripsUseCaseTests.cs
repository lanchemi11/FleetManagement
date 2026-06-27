using FleetManagement.Modules.Drivers.Application;
using FleetManagement.Modules.Drivers.Domain;
using FleetManagement.Modules.Trips.Application;
using FleetManagement.Modules.Trips.Domain;
using FleetManagement.Modules.Vehicles.Application;
using FleetManagement.Modules.Vehicles.Domain;
using FleetManagement.SharedKernel.Events;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Tests
{
    public class TripsUseCaseTests
    {
        private readonly Mock<ITripRepository> _tripRepositoryMock;
        private readonly Mock<IVehicleRepository> _vehicleRepositoryMock;
        private readonly Mock<IDriverRepository> _driverRepositoryMock;
        private readonly Mock<IEventBus> _eventBusMock;

        public TripsUseCaseTests()
        {
            _tripRepositoryMock = new Mock<ITripRepository>();
            _vehicleRepositoryMock = new Mock<IVehicleRepository>();
            _driverRepositoryMock = new Mock<IDriverRepository>();
            _eventBusMock = new Mock<IEventBus>();
        }

        [Fact]
        public async Task StartTrip_ShouldSucceed_WhenVehicleAndDriverAreAvailable()
        {
            var vehicle = new Vehicle("Skoda", "Octavia", "BG-123-XX", 10000);
            var driver = new Driver("Petar", "Petrovic", "123456");

            _vehicleRepositoryMock.Setup(x => x.GetByIdAsync(vehicle.Id)).ReturnsAsync(vehicle);
            _driverRepositoryMock.Setup(x => x.GetByIdAsync(driver.Id)).ReturnsAsync(driver);

            var useCase = new StartTripUseCase(
                _tripRepositoryMock.Object,
                _vehicleRepositoryMock.Object,
                _driverRepositoryMock.Object
            );

            var command = new StartTripCommand(vehicle.Id, driver.Id, "Beograd", "Nis");

            await useCase.ExecuteAsync(command);

            Assert.Equal(VehicleStatus.InTrip, vehicle.Status);
            Assert.Equal(DriverStatus.Active, driver.Status);

            _vehicleRepositoryMock.Verify(x => x.UpdateAsync(vehicle), Times.Once);
            _driverRepositoryMock.Verify(x => x.UpdateAsync(driver), Times.Once);
            _tripRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Trip>()), Times.Once);
        }

        [Fact]
        public async Task StartTrip_ShouldThrowException_WhenVehicleIsAlreadyInTrip()
        {
            var vehicle = new Vehicle("Skoda", "Octavia", "BG-123-XX", 10000);
            vehicle.AssignToTrip();

            var driver = new Driver("Petar", "Petrovic", "123456");

            _vehicleRepositoryMock.Setup(x => x.GetByIdAsync(vehicle.Id)).ReturnsAsync(vehicle);
            _driverRepositoryMock.Setup(x => x.GetByIdAsync(driver.Id)).ReturnsAsync(driver);

            var useCase = new StartTripUseCase(
                _tripRepositoryMock.Object,
                _vehicleRepositoryMock.Object,
                _driverRepositoryMock.Object
            );

            var command = new StartTripCommand(vehicle.Id, driver.Id, "Beograd", "Nis");

            await Assert.ThrowsAsync<InvalidOperationException>(() => useCase.ExecuteAsync(command));
        }

        [Fact]
        public async Task StartTrip_ShouldThrowException_WhenDriverIsAlreadyActive()
        {
            var vehicle = new Vehicle("Skoda", "Octavia", "BG-123-XX", 10000);
            var driver = new Driver("Petar", "Petrovic", "123456");
            driver.AssignToTrip();

            _vehicleRepositoryMock.Setup(x => x.GetByIdAsync(vehicle.Id)).ReturnsAsync(vehicle);
            _driverRepositoryMock.Setup(x => x.GetByIdAsync(driver.Id)).ReturnsAsync(driver);

            var useCase = new StartTripUseCase(
                _tripRepositoryMock.Object,
                _vehicleRepositoryMock.Object,
                _driverRepositoryMock.Object
            );

            var command = new StartTripCommand(vehicle.Id, driver.Id, "Beograd", "Nis");

            await Assert.ThrowsAsync<InvalidOperationException>(() => useCase.ExecuteAsync(command));
        }

        [Fact]
        public async Task CompleteTrip_ShouldSucceed_AndReleaseResources()
        {
            var vehicle = new Vehicle("Skoda", "Octavia", "BG-123-XX", 10000);
            vehicle.AssignToTrip();

            var driver = new Driver("Petar", "Petrovic", "123456");
            driver.AssignToTrip();

            var trip = new Trip(vehicle.Id, driver.Id, "Beograd", "Nis", 10000);

            _tripRepositoryMock.Setup(x => x.GetByIdAsync(trip.Id)).ReturnsAsync(trip);
            _vehicleRepositoryMock.Setup(x => x.GetByIdAsync(vehicle.Id)).ReturnsAsync(vehicle);
            _driverRepositoryMock.Setup(x => x.GetByIdAsync(driver.Id)).ReturnsAsync(driver);

            var useCase = new CompleteTripUseCase(
                _tripRepositoryMock.Object,
                _vehicleRepositoryMock.Object,
                _driverRepositoryMock.Object,
                _eventBusMock.Object
            );

            var command = new CompleteTripCommand(trip.Id, 10250);

            await useCase.ExecuteAsync(command);

            Assert.True(trip.IsCompleted);
            Assert.Equal(10250, vehicle.CurrentMileage);          
            Assert.Equal(VehicleStatus.Available, vehicle.Status);
            Assert.Equal(DriverStatus.Available, driver.Status);

            _eventBusMock.Verify(x => x.PublishAsync(It.IsAny<TripCompletedEvent>()), Times.Once);
        }

        [Fact]
        public async Task CompleteTrip_ShouldThrowException_WhenEndMileageIsLessThanStart()
        {
            var vehicle = new Vehicle("Skoda", "Octavia", "BG-123-XX", 10000);
            vehicle.AssignToTrip();

            var driver = new Driver("Petar", "Petrovic", "123456");
            driver.AssignToTrip();

            var trip = new Trip(vehicle.Id, driver.Id, "Beograd", "Nis", 10000);

            _tripRepositoryMock.Setup(x => x.GetByIdAsync(trip.Id)).ReturnsAsync(trip);
            _vehicleRepositoryMock.Setup(x => x.GetByIdAsync(vehicle.Id)).ReturnsAsync(vehicle);
            _driverRepositoryMock.Setup(x => x.GetByIdAsync(driver.Id)).ReturnsAsync(driver);

            var useCase = new CompleteTripUseCase(
                _tripRepositoryMock.Object,
                _vehicleRepositoryMock.Object,
                _driverRepositoryMock.Object,
                _eventBusMock.Object
            );

            var command = new CompleteTripCommand(trip.Id, 9900);

            await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(command));
        }
    }
}
