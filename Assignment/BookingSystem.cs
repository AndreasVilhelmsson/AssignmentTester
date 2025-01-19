using System;
using System.Collections.Generic;
using System.Linq;

public class BookingSystem
{
	private readonly IBookingRepository _bookingRepository;

	public BookingSystem(IBookingRepository bookingRepository)
	{
		_bookingRepository = bookingRepository;
	}

	public List<IBooking> Bookings => _bookingRepository.GetBookings();

	public bool BookTimeSlot(DateTime startTime, DateTime endTime)
	{
		// Kontrollera att starttiden inte är i det förflutna
		if (startTime < DateTime.Now)
		{
			return false;
		}

		// Kontrollera att sluttiden inte är i det förflutna
		if (endTime < DateTime.Now)
		{
			return false;
		}

		// Kontrollera att starttiden är före sluttiden
		if (startTime >= endTime)
		{
			return false;
		}

		// Kontrollera att ingen befintlig bokning överlappar med den nya bokningen
		foreach (var booking in Bookings)
		{
			if ((startTime < booking.EndTime) && (endTime > booking.StartTime))
			{
				return false; // Bokningen är inte möjlig
			}
		}

		// Lägg till den nya bokningen
		_bookingRepository.SaveBooking(new Booking { StartTime = startTime, EndTime = endTime });
		return true; // Bokningen är möjlig
	}

	public List<DateTime> GetAvailableTimeSlots()
	{
		var availableSlots = new List<DateTime>();
		var currentTime = DateTime.Now;

		// Lägg till alla tidsluckor som inte överlappar med befintliga bokningar
		for (int i = 0; i < 24; i++)
		{
			var slotStartTime = currentTime.Date.AddHours(i);
			var slotEndTime = slotStartTime.AddHours(1);

			if (!Bookings.Any(b => (slotStartTime < b.EndTime) && (slotEndTime > b.StartTime)))
			{
				availableSlots.Add(slotStartTime);
			}
		}

		return availableSlots;
	}
}