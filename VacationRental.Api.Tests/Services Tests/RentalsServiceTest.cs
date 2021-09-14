using System;
using System.Collections.Generic;
using VacationRental.Api.Models;
using VacationRental.Api.Services;
using Xunit;

namespace VacationRental.Api.Tests
{
    public class RentalsServiceTest
    {
        private readonly RentalService rentalService;
        private readonly RentalBindingModel testModel;
        private readonly ResourceIdViewModel testKey;
        private const int ID = 1;

        public RentalsServiceTest()
        {
            var dict = new Dictionary<int, RentalViewModel>();
            rentalService = new RentalService(dict);
            testModel = new RentalBindingModel() { PreparationTimeInDays = 1, Units = 1 };
            testKey = new ResourceIdViewModel() { Id = ID };
        }

        [Fact]
        public void FindThrowsExceptionIfDictionaryDoesNotContainTheGivenKey()
        {
            Assert.Throws<ApplicationException>(() => rentalService.Find(ID));
        }

        [Fact]
        public void AddAddsOneElementToTheDictionary()
        {
            Assert.Throws<ApplicationException>(() => rentalService.Find(ID));

            rentalService.Add(testModel, testKey);

            Assert.NotNull(rentalService.Find(ID));
        }

        [Fact]
        public void AddThrowsExceptionWhenKeyAlreadyExists()
        {
            rentalService.Add(testModel, testKey);
            Assert.Throws<ArgumentException>(() => rentalService.Add(testModel, testKey));
        }

        [Fact]
        public void GetNextKeyReturnsTheNextIdValue()
        {
            Assert.Equal(1, rentalService.GetNextKey());

            rentalService.Add(testModel, testKey);

            Assert.Equal(2, rentalService.GetNextKey());
        }

        [Fact]
        public void FindReturnsTheRequestedObjectIfItExistsInDictionary()
        {
            var expected = new RentalViewModel() { Id = ID, PreparationTimeInDays = testModel.PreparationTimeInDays, Units = testModel.Units };

            Assert.Throws<ApplicationException>(() => rentalService.Find(ID));

            rentalService.Add(testModel, testKey);
            var actual = rentalService.Find(ID);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UpdateFailsIfTheObjectDoesNotExistInDictionary()
        {
            Assert.Throws<ApplicationException>(() => rentalService.Update(ID, testModel));
        }

        [Fact]
        public void UpdateModifiesAnObject()
        {
            rentalService.Add(testModel, testKey);

            Assert.Equal(1, rentalService.Find(ID).PreparationTimeInDays);
            Assert.Equal(1, rentalService.Find(ID).Units);

            var modifiedModel = new RentalBindingModel() { PreparationTimeInDays = 2, Units = 2 };

            rentalService.Update(ID, modifiedModel);

            Assert.Equal(2, rentalService.Find(ID).PreparationTimeInDays);
            Assert.Equal(2, rentalService.Find(ID).Units);
        }
    }
}
