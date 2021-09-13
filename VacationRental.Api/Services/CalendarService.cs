using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
