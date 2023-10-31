

using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

using Uqs.AppointmentBooking.Domain.Services;
using Uqs.AppointmentBooking.Domain.Tests.Unit.Fakes;



namespace Uqs.AppointmentBooking.Domain.Tests.Unit
{
    public class SlotsServiceTests : IDisposable
    {
        private readonly ApplicationContextFakeBuilder _contextBuilder = new();
        private SlotsService? _sut;
        private readonly INowService _nowService = Substitute.For<INowService>();
        private readonly IOptions<ApplicationSettings> _settings =
       Substitute.For<IOptions<ApplicationSettings>>();


        public void Dispose()
        {
            _contextBuilder.Dispose();
        }

        [Fact]
        public async Task GetAvailableSlotsForEmployee_ServiceIdNoFound_ArgumentException()
        {
            // Arrange
            var context = _contextBuilder.Build();

            _sut = new SlotsService(context, _nowService, _settings);

            // Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetAvailableSlotsForEmployee(-1, -1));

            // Assert
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public async void GetAvailableSlotsForEmployee_NoShiftsForTomAndNoAppointmentsInSystem_NoSlots()
        {
            // Arrange
            var appointmentFrom =
            new DateTime(2022, 10, 3, 7, 0, 0);
            _nowService.Now.Returns(appointmentFrom);
            var context = _contextBuilder
            .WithSingleService(30)
            .WithSingleEmployeeTom()
            .Build();
            _sut = new SlotsService(context, _nowService, _settings);
            var tom = context.Employees!.Single();
            var mensCut30Min = context.Services!.Single();
            // Act
            var slots = await
            _sut.GetAvailableSlotsForEmployee(
            mensCut30Min.Id, tom.Id);
            // Assert
            var times = slots.DaysSlots.SelectMany(x => x.Times);
            Assert.Empty(times);

        }
    }
}
