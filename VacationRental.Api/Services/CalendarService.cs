using System;
using System.Collections.Generic;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services
{
    public class CalendarService
    {
        private readonly BookingService bookingService;
        private readonly RentalService rentalService;

        public CalendarService(IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings)
        {
            bookingService = new BookingService(rentals, bookings);
            rentalService = new RentalService(rentals);
        }

        public CalendarViewModel CreateCalendarView(int rentalId, DateTime start, int nights)
        {
            Validate(nights, rentalId);

            var dates = new List<CalendarDateViewModel>();

            for (int i = 0; i < nights; i++)
            {
                var date = CreateView(start, i);

                FindRentalBookings(rentalId, date);

                dates.Add(date);
            }

            return new CalendarViewModel()
            {
                RentalId = rentalId,
                Dates = dates
            };
        }

        private void FindRentalBookings(int rentalId, CalendarDateViewModel date)
        {
            foreach (var booking in bookingService.GetBookingsByRentalId(rentalId).Values)
            {
                var rental = rentalService.Find(rentalId);

                if (booking.Start <= date.Date && booking.Start.AddDays(booking.Nights + rental.PreparationTimeInDays) > date.Date)
                {
                    date.Bookings.Add(new CalendarBookingViewModel(booking.Id, booking.Unit));
                    date.PreparationTimes.Add(new PreparationTimeViewModel() { Unit = booking.Unit });
                }
            }
        }

        private static CalendarDateViewModel CreateView(DateTime start, int i)
        {
            return new CalendarDateViewModel
            {
                Date = start.Date.AddDays(i),
                Bookings = new List<CalendarBookingViewModel>(),
                PreparationTimes = new List<PreparationTimeViewModel>()
            };
        }

        private void Validate(int nights, int rentalId)
        {
            if (nights < 0)
                throw new ApplicationException("Nights must be positive");
            _ = rentalService.Find(rentalId);
        }
    }
}
