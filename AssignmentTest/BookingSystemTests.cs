using System;
using System.Collections.Generic;
using Xunit;
using Moq;

public class BookingSystemTests
{
	private readonly Mock<IBookingRepository> _mockBookingRepository;
	private readonly BookingSystem _bookingSystem;

	public BookingSystemTests()
	{
		_mockBookingRepository = new Mock<IBookingRepository>();
		_bookingSystem = new BookingSystem(_mockBookingRepository.Object);
	}

	[Fact]
	public void BookTimeSlot_ShouldAddBooking_WhenNoOverlap()
	{
		// Arrange
		DateTime startTime = new DateTime(2025, 1, 20, 10, 0, 0);
		DateTime endTime = new DateTime(2025, 1, 20, 11, 0, 0);
		_mockBookingRepository.Setup(repo => repo.GetBookings()).Returns(new List<IBooking>());

		// Act
		bool result = _bookingSystem.BookTimeSlot(startTime, endTime);

		// Assert
		Assert.True(result);
		_mockBookingRepository.Verify(repo => repo.SaveBooking(It.Is<IBooking>(b => b.StartTime == startTime && b.EndTime == endTime)), Times.Once);
	}

	[Fact]
	public void BookTimeSlot_ShouldNotAllowOverlappingBookings()
	{
		// Arrange
		DateTime startTime1 = new DateTime(2025, 1, 20, 10, 0, 0);
		DateTime endTime1 = new DateTime(2025, 1, 20, 11, 0, 0);
		DateTime startTime2 = new DateTime(2025, 1, 20, 10, 30, 0);
		DateTime endTime2 = new DateTime(2025, 1, 20, 11, 30, 0);

		var existingBookings = new List<IBooking>
		{
			new Booking { StartTime = startTime1, EndTime = endTime1 }
		};
		_mockBookingRepository.Setup(repo => repo.GetBookings()).Returns(existingBookings);

		// Act
		bool result = _bookingSystem.BookTimeSlot(startTime2, endTime2);

		// Assert
		Assert.False(result);
		_mockBookingRepository.Verify(repo => repo.SaveBooking(It.IsAny<IBooking>()), Times.Never);
	}

	[Fact]
	public void GetAvailableTimeSlots_ShouldReturnCorrectAvailableTimes()
	{
		// Arrange
		DateTime currentTime = DateTime.Now.Date;
		DateTime startTime = currentTime.AddHours(10);
		DateTime endTime = currentTime.AddHours(11);

		var existingBookings = new List<IBooking>
		{
			new Booking { StartTime = startTime, EndTime = endTime }
		};
		_mockBookingRepository.Setup(repo => repo.GetBookings()).Returns(existingBookings);

		// Act
		var availableSlots = _bookingSystem.GetAvailableTimeSlots();

		// Assert
		Assert.DoesNotContain(startTime, availableSlots);
		Assert.Contains(currentTime.AddHours(12), availableSlots);
	}

	[Fact]
	public void BookTimeSlot_ShouldNotAllowBooking_WhenStartTimeIsAfterEndTime()
	{
		// Arrange
		DateTime startTime = new DateTime(2025, 1, 20, 11, 0, 0);
		DateTime endTime = new DateTime(2025, 1, 20, 10, 0, 0);
		_mockBookingRepository.Setup(repo => repo.GetBookings()).Returns(new List<IBooking>());

		// Act
		bool result = _bookingSystem.BookTimeSlot(startTime, endTime);

		// Assert
		Assert.False(result);
		_mockBookingRepository.Verify(repo => repo.SaveBooking(It.IsAny<IBooking>()), Times.Never);
	}

	// Additional Crash Cases
	[Fact]
	public void BookTimeSlot_ShouldNotAllowBooking_WhenStartTimeIsEqualToEndTime()
	{
		// Arrange
		DateTime startTime = new DateTime(2025, 1, 20, 10, 0, 0);
		DateTime endTime = new DateTime(2025, 1, 20, 10, 0, 0);
		_mockBookingRepository.Setup(repo => repo.GetBookings()).Returns(new List<IBooking>());

		// Act
		bool result = _bookingSystem.BookTimeSlot(startTime, endTime);

		// Assert
		Assert.False(result);
		_mockBookingRepository.Verify(repo => repo.SaveBooking(It.IsAny<IBooking>()), Times.Never);
	}

	[Fact]
	public void BookTimeSlot_ShouldNotAllowBooking_WhenStartTimeIsInThePast()
	{
		// Arrange
		DateTime startTime = DateTime.Now.AddHours(-1);
		DateTime endTime = DateTime.Now.AddHours(1);
		_mockBookingRepository.Setup(repo => repo.GetBookings()).Returns(new List<IBooking>());

		// Act
		bool result = _bookingSystem.BookTimeSlot(startTime, endTime);

		// Assert
		Assert.False(result);
		_mockBookingRepository.Verify(repo => repo.SaveBooking(It.IsAny<IBooking>()), Times.Never);
	}

	[Fact]
	public void BookTimeSlot_ShouldNotAllowBooking_WhenEndTimeIsInThePast()
	{
		// Arrange
		DateTime startTime = DateTime.Now;
		DateTime endTime = DateTime.Now.AddHours(-1);
		_mockBookingRepository.Setup(repo => repo.GetBookings()).Returns(new List<IBooking>());

		// Act
		bool result = _bookingSystem.BookTimeSlot(startTime, endTime);

		// Assert
		Assert.False(result);
		_mockBookingRepository.Verify(repo => repo.SaveBooking(It.IsAny<IBooking>()), Times.Never);
	}
}