using System;
using System.Collections.Generic;
using VacationRental.Api.Models;
using VacationRental.Api.Services;
using Xunit;

namespace VacationRental.Api.Tests
{
    public class BookingServiceTest
    {
        private BookingService bookingsService;
        private readonly BookingBindingModel testModel;
        private readonly ResourceIdViewModel testKey;
        private readonly RentalViewModel testRental;
        private readonly Dictionary<int, RentalViewModel> rentals;
        private readonly Dictionary<int, BookingViewModel> bookings;
        private const int ID = 1;

        public BookingServiceTest()
        {
            testRental = new RentalViewModel(ID, 1, 1);
            rentals = new Dictionary<int, RentalViewModel>
            {
                { ID, testRental }
            };

            bookings = new Dictionary<int, BookingViewModel>();

            bookingsService = new BookingService(rentals, bookings);

            testModel = new BookingBindingModel() { Nights = 1, RentalId = 1, Start = DateTime.Today, Unit = 1 };
            testKey = new ResourceIdViewModel(ID);
        }

        [Fact]
        public void FindThrowsExceptionIfDictionaryDoesNotContainTheGivenKey()
        {
            bookings.Clear();
            bookingsService = new BookingService(rentals, bookings);

            Assert.Throws<ApplicationException>(() => bookingsService.Find(ID));
        }

        [Fact]
        public void AddFailsIfRentalDoesNotExist()
        {
            var bindingModel = new BookingBindingModel() { Nights = 1, RentalId = 2, Start = DateTime.Today, Unit = 1 };
            var exception = Assert.Throws<ApplicationException>(() => bookingsService.Add(bindingModel, testKey));

            Assert.Equal("Rental not found", exception.Message);
        }

        [Fact]
        public void AddFailsIfNightsIsZeroOrLess()
        {
            var invalidModel = new BookingBindingModel() { Nights = 0, RentalId = ID, Start = DateTime.Today, Unit = 1 };

            var exception = Assert.Throws<ApplicationException>(() => bookingsService.Add(invalidModel, testKey));
            Assert.Equal("Nigts must be positive", exception.Message);
        }

        [Fact]
        public void AddFailsIfModelHasMoreUnitsThanRental()
        {
            var mod = new BookingViewModel(ID, ID, DateTime.Today, 1, 1);
            bookings.Add(2, mod);

            var tempBookingsRepository = new BookingService(rentals, bookings);

            var exception = Assert.Throws<ApplicationException>(() => tempBookingsRepository.Add(testModel, testKey));
            Assert.Equal("Not available", exception.Message);
        }

        [Fact]
        public void AddAddsOneElementToTheDictionary()
        {
            Assert.Throws<ApplicationException>(() => bookingsService.Find(ID));

            bookingsService.Add(testModel, testKey);

            Assert.NotNull(bookingsService.Find(ID));
        }

        [Fact]
        public void GetNextKeyReturnsTheNextIdValue()
        {
            Assert.Equal(1, bookingsService.GetNextKey());

            bookingsService.Add(testModel, testKey);

            Assert.Equal(2, bookingsService.GetNextKey());
        }

        [Fact]
        public void FindReturnsTheRequestedObjectIfItExistsInDictionary()
        {
            var expected = new BookingViewModel(ID, testModel.RentalId, testModel.Start, testModel.Nights, testModel.Unit);

            Assert.Throws<ApplicationException>(() => bookingsService.Find(ID));

            bookingsService.Add(testModel, testKey);
            var actual = bookingsService.Find(ID);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UpdateFailsIfTheObjectDoesNotExistInDictionary()
        {
            Assert.Throws<ApplicationException>(() => bookingsService.Update(ID, testModel));
        }

        [Fact]
        public void UpdateFailsIfTheNewValueIsAlreadyBooked()
        {
            bookingsService.Add(testModel, testKey);

            var blockingModel = testModel;
            blockingModel.Start = DateTime.Today.AddDays(testModel.Nights + testRental.PreparationTimeInDays);

            bookingsService.Add(blockingModel, new ResourceIdViewModel(bookingsService.GetNextKey()));

            var modifiedModel = testModel;
            modifiedModel.Nights = 2;

            var exception = Assert.Throws<ApplicationException>(() => bookingsService.Update(ID, modifiedModel));
            Assert.Equal("Not available", exception.Message);
        }

        [Fact]
        public void UpdateModifiesAnObject()
        {
            bookingsService.Add(testModel, testKey);

            Assert.Equal(1, bookingsService.Find(ID).Nights);
            Assert.Equal(DateTime.Today, bookingsService.Find(ID).Start);

            var modifiedModel = new BookingBindingModel() { Nights = 2, RentalId = ID, Start = DateTime.MinValue };

            bookingsService.Update(ID, modifiedModel);

            Assert.Equal(2, bookingsService.Find(ID).Nights);
            Assert.Equal(DateTime.MinValue, bookingsService.Find(ID).Start);
        }

        [Fact]
        public void FindBookingsByIdReturnsByRequestedId()
        {
            var expectedBooking = new BookingViewModel(ID, ID, DateTime.Today, 1, 1);
            var expectedDictionary = new Dictionary<int, BookingViewModel>()
            {
                {1, expectedBooking }
            };
            var ignoredBooking = new BookingViewModel(ID, 2, DateTime.Today, 1, 1);
            var bookingsDictionary = new Dictionary<int, BookingViewModel>
            {
                { 1, expectedBooking },
                { 2, ignoredBooking }
            };
            bookingsService = new BookingService(rentals, bookingsDictionary);

            var actual = bookingsService.GetBookingsByRentalId(ID);

            Assert.Equal(expectedDictionary, actual);
        }
    }
}
