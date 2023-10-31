using Uqs.AppointmentBooking.Domain.Services;
using Uqs.AppointmentBooking.Domain.Tests.Unit.Fakes;
using Xunit;

namespace Uqs.AppointmentBooking.Domain.Tests.Unit;

public class ServicesServiceTests: IDisposable
{
    private readonly ApplicationContextFakeBuilder _contextBuilder = new();
    private ServicesService? _sut;

    public void Dispose()
    {
        _contextBuilder.Dispose();
    }

	[Fact]
	public async void GetActiveServices_NoServicesInTheSystem_NoServices()
	{
		// Arrange
		var context = _contextBuilder.Build();

		_sut = new ServicesService(context);
		// Act
		var actual = await _sut.GetActivesServices();

        // Assert
        Assert.True(!actual.Any());

    }

	[Fact]
	public async void GetActiveServices_TwoActiveOneInactiveService_TwoServices()
	{
		// Arrange
		var context = _contextBuilder
			.WithSingleService(true)
			.WithSingleService(true)
            .WithSingleService(false)
			.Build();

		_sut = new ServicesService(context);

		var expected = 2;

		// Act
		var actual = await _sut.GetActivesServices();

		// Assert
		Assert.Equal(expected, actual.Count());
	}

   
}