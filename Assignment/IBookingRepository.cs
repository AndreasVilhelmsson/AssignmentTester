using System;
using System.Collections.Generic;

public interface IBookingRepository
{
	List<IBooking> GetBookings();
	void SaveBooking(IBooking booking);
}