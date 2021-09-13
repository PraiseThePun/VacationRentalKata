using System;
using System.Collections.Generic;
using VacationRental.Api.Models;
using VacationRental.Api.Repos;

namespace VacationRental.Api.Services
{
    public class CalendarService
    {
        private readonly BookingsRepository bookingsRepository;

        public CalendarService(IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings)
        {
            bookingsRepository = new BookingsRepository(rentals, bookings);
        }

        public CalendarViewModel CreateCalendarView(int rentalId, DateTime start, int nights)
        {
            Validate(nights, rentalId);

            var dates = new List<CalendarDateViewModel>();

            for (int i = 0; i < nights; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>(),
                    PreparationTimes = new List<PreparationTimeViewModel>()
                };

                foreach (var booking in bookingsRepository.GetAllBookings().Values)
                {
                    var rental = bookingsRepository.FindRental(booking.RentalId);

                    if (booking.RentalId == rentalId
                        && booking.Start <= date.Date && booking.Start.AddDays(booking.Nights + rental.PreparationTimeInDays) > date.Date)
                    {
                        date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id, Unit = booking.Unit });
                        date.PreparationTimes.Add(new PreparationTimeViewModel() { Unit = booking.Unit });
                    }

                    dates.Add(date);
                }
            }

            return new CalendarViewModel()
            {
                RentalId = rentalId,
                Dates = dates
            };
        }

        private void Validate(int nights, int rentalId)
        {
            if (nights < 0)
                throw new ApplicationException("Nights must be positive");
            _ = bookingsRepository.FindRental(rentalId);
        }
    }
}
