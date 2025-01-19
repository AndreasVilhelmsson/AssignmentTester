using System;
using System.Collections.Generic;

public class InMemoryBookingRepository : IBookingRepository
{
	private readonly List<IBooking> _bookings = new List<IBooking>();

	public List<IBooking> GetBookings()
	{
		return _bookings;
	}

	public void SaveBooking(IBooking booking)
	{
		_bookings.Add(booking);
	}
}