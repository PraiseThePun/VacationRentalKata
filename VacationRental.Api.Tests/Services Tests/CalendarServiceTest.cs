using System;
using System.Collections.Generic;
using VacationRental.Api.Models;
using VacationRental.Api.Services;
using Xunit;

namespace VacationRental.Api.Tests.Services_Tests
{
    public class CalendarServiceTest
    {
        private readonly CalendarService calendarService;
        private readonly RentalViewModel testRental;
        private readonly Dictionary<int, RentalViewModel> rentals;
        private readonly Dictionary<int, BookingViewModel> bookings;
        private const int ID = 1;

        public CalendarServiceTest()
        {
            testRental = new RentalViewModel() { Id = ID, PreparationTimeInDays = 1, Units = 1 };
            rentals = new Dictionary<int, RentalViewModel>
            {
                { ID, testRental }
            };

            var testBooking = new BookingViewModel(ID, ID, DateTime.Today, 1, 1);
            bookings = new Dictionary<int, BookingViewModel>
            {
                { ID, testBooking }
            };

            calendarService = new CalendarService(rentals, bookings);
        }

        [Fact]
        public void CreateCalendarFailsIfNightsIsNegativeValue()
        {
            var exception = Assert.Throws<ApplicationException>(() => calendarService.CreateCalendarView(ID, DateTime.Today, -1));
            Assert.Equal("Nights must be positive", exception.Message);
        }

        [Fact]
        public void CreateCalendarFailsIfRentalDoesNotExist()
        {
            var exception = Assert.Throws<ApplicationException>(() => calendarService.CreateCalendarView(2, DateTime.Today, 1));
            Assert.Equal("Rental not found", exception.Message);
        }

        [Fact]
        public void CreateCalendarReturnsAViewModel()
        {
            var bookings = new List<CalendarBookingViewModel>() { new CalendarBookingViewModel(1, 1) };
            var preparationTimes = new List<PreparationTimeViewModel>() { new PreparationTimeViewModel(1) };
            var dates = new List<CalendarDateViewModel>() { new CalendarDateViewModel(DateTime.Today, bookings, preparationTimes) };
            var expected = new CalendarViewModel(ID, dates);

            var actual = calendarService.CreateCalendarView(1, DateTime.Today, 1);

            Assert.Equal(expected, actual);
        }
    }
}
