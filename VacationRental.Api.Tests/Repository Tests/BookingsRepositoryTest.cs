using System;
using System.Collections.Generic;
using VacationRental.Api.Models;
using VacationRental.Api.Repos;
using Xunit;

namespace VacationRental.Api.Tests
{
    public class BookingsRepositoryTest
    {
        private BookingsRepository bookingsRepository;
        private readonly BookingBindingModel testModel;
        private readonly ResourceIdViewModel testKey;
        private readonly RentalViewModel testRental;
        private readonly Dictionary<int, RentalViewModel> rentals;
        private readonly Dictionary<int, BookingViewModel> bookings;
        private const int ID = 1;

        public BookingsRepositoryTest()
        {
            testRental = new RentalViewModel() { Id = ID, PreparationTimeInDays = 1, Units = 1 };
            rentals = new Dictionary<int, RentalViewModel>
            {
                { ID, testRental }
            };
            bookings = new Dictionary<int, BookingViewModel>();

            bookingsRepository = new BookingsRepository(rentals, bookings);

            testModel = new BookingBindingModel() { Nights = 1, RentalId = 1, Start = DateTime.Today };
            testKey = new ResourceIdViewModel() { Id = ID };
        }

        [Fact]
        public void FindThrowsExceptionIfDictionaryDoesNotContainTheGivenKey()
        {
            Assert.Throws<ApplicationException>(() => bookingsRepository.Find(ID));
        }

        [Fact]
        public void AddFailsIfRentalDoesNotExist()
        {
            rentals.Clear();
            bookingsRepository = new BookingsRepository(rentals, bookings);

            var exception = Assert.Throws<ApplicationException>(() => bookingsRepository.Add(testModel, testKey));
            Assert.Equal("Rental not found", exception.Message);
        }

        [Fact]
        public void AddFailsIfNightsIsZeroOrLess()
        {
            var invalidModel = new BookingBindingModel() { Nights = 0, RentalId = ID, Start = DateTime.Today };

            var exception = Assert.Throws<ApplicationException>(() => bookingsRepository.Add(invalidModel, testKey));
            Assert.Equal("Nigts must be positive", exception.Message);
        }

        [Fact]
        public void AddAddsOneElementToTheDictionary()
        {
            Assert.Throws<ApplicationException>(() => bookingsRepository.Find(ID));

            bookingsRepository.Add(testModel, testKey);

            Assert.NotNull(bookingsRepository.Find(ID));
        }

        [Fact]
        public void GetNextKeyReturnsTheNextIdValue()
        {
            Assert.Equal(1, bookingsRepository.GetNextKey());

            bookingsRepository.Add(testModel, testKey);

            Assert.Equal(2, bookingsRepository.GetNextKey());
        }

        [Fact]
        public void FindReturnsTheRequestedObjectIfItExistsInDictionary()
        {
            var expected = new BookingViewModel() { Id = ID, Nights = testModel.Nights, RentalId = testModel.RentalId, Start = testModel.Start };

            Assert.Throws<ApplicationException>(() => bookingsRepository.Find(ID));

            bookingsRepository.Add(testModel, testKey);
            var actual = bookingsRepository.Find(ID);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UpdateFailsIfTheObjectDoesNotExistInDictionary()
        {
            Assert.Throws<ApplicationException>(() => bookingsRepository.Update(ID, testModel));
        }

        [Fact]
        public void UpdateModifiesAnObject()
        {
            bookingsRepository.Add(testModel, testKey);

            Assert.Equal(1, bookingsRepository.Find(ID).Nights);
            Assert.Equal(DateTime.Today, bookingsRepository.Find(ID).Start);

            var modifiedModel = new BookingBindingModel() { Nights = 2, RentalId = ID, Start = DateTime.MinValue };

            bookingsRepository.Update(ID, modifiedModel);

            Assert.Equal(2, bookingsRepository.Find(ID).Nights);
            Assert.Equal(DateTime.MinValue, bookingsRepository.Find(ID).Start);
        }
    }
}
