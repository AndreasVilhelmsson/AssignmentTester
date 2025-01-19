using System;

public class BookingServiceFacade
{
	private BookingSystem _bookingSystem;

	public BookingServiceFacade(IBookingRepository bookingRepository)
	{
		_bookingSystem = new BookingSystem(bookingRepository);
	}

	// Metod för att boka en tidslucka via facaden
	public bool BookSlot(DateTime startTime, DateTime endTime)
	{
		return _bookingSystem.BookTimeSlot(startTime, endTime);
	}

	// Metod för att hämta tillgängliga tidsluckor via facaden
	public List<DateTime> GetAvailableTimeSlots()
	{
		return _bookingSystem.GetAvailableTimeSlots();
	}
}