using System;
using System.Collections.Generic;
using System.Linq;
using LiveCoding.Domain;
using LiveCoding.Persistence;

namespace LiveCoding.Tests;

public class FakeBookingRepository : IBookingRepository
{
    private readonly List<Booking> _bookings = new();

    public IEnumerable<Booking> GetUpcomingBookings()
    {
        return _bookings;
    }

    public Booking GetUpcomingBooking(DateTime date)
    {
        return _bookings.First(r => r.Date == date);
    }

    public void Save(Booking booking)
    {
        _bookings.Add(booking);
    }
}