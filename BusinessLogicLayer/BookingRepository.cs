using SmartHotelBookingSystem.DataAccess.EFCore;
using SmartHotelBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartHotelBookingSystem.BusinessLogicLayer
{
    public class BookingRepository
    {
        private readonly AppDbContext _context;

        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddBooking(Booking booking)
        {
            _context.Bookings.Add(booking);
            _context.SaveChanges();
        }

        public List<Booking> GetAllBookings()
        {
            return _context.Bookings.ToList();
        }

        public List<Booking> GetBookingsByBookingID(int bookingID)
        {
            return _context.Bookings.Where(a => a.BookingID == bookingID).ToList();
        }

        public void UpdateBooking(int bookingID, DateTime checkInDate)
        {
            var existingBooking = _context.Bookings.Find(bookingID);
            if (existingBooking != null)
            {
                existingBooking.CheckInDate = checkInDate;
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Booking not found.");
            }
        }

        public void DeleteBooking(int bookingID)
        {
            var bookingToDelete = _context.Bookings.Find(bookingID);
            if (bookingToDelete != null)
            {
                _context.Bookings.Remove(bookingToDelete);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Booking not found.");
            }
        }
    }
}