using System;
using System.Collections.Generic;
using VacationRental.Api.Models;
using VacationRental.Api.Repos;
using Xunit;

namespace VacationRental.Api.Tests
{
    public class RentalsControllerTest
    {
        private readonly RentalsRepository rentalsRepository;
        private readonly RentalBindingModel testModel;
        private readonly ResourceIdViewModel testKey;
        private const int ID = 1;

        public RentalsControllerTest()
        {
            var dict = new Dictionary<int, RentalViewModel>();
            rentalsRepository = new RentalsRepository(dict);
            testModel = new RentalBindingModel() { PreparationTimeInDays = 1, Units = 1 };
            testKey = new ResourceIdViewModel() { Id = ID };
        }

        [Fact]
        public void FindThrowsExceptionIfDictionaryDoesNotContainTheGivenKey()
        {
            Assert.Throws<ApplicationException>(() => rentalsRepository.Find(ID));
        }

        [Fact]
        public void AddAddsOneElementToTheDictionary()
        {
            Assert.Throws<ApplicationException>(() => rentalsRepository.Find(ID));

            rentalsRepository.Add(testModel, testKey);

            Assert.NotNull(rentalsRepository.Find(ID));
        }

        [Fact]
        public void AddThrowsExceptionWhenKeyAlreadyExists()
        {
            rentalsRepository.Add(testModel, testKey);
            Assert.Throws<ArgumentException>(() => rentalsRepository.Add(testModel, testKey));
        }

        [Fact]
        public void GetNextKeyReturnsTheNextIdValue()
        {
            Assert.Equal(1, rentalsRepository.GetNextKey());

            rentalsRepository.Add(testModel, testKey);

            Assert.Equal(2, rentalsRepository.GetNextKey());
        }

        [Fact]
        public void FindReturnsTheRequestedObjectIfItExistsInDictionary()
        {
            var expected = new RentalViewModel() { Id = ID, PreparationTimeInDays = testModel.PreparationTimeInDays, Units = testModel.Units };

            Assert.Throws<ApplicationException>(() => rentalsRepository.Find(ID));

            rentalsRepository.Add(testModel, testKey);
            var actual = rentalsRepository.Find(ID);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UpdateFailsIfTheObjectDoesNotExistInDictionary()
        {
            Assert.Throws<ApplicationException>(() => rentalsRepository.Update(ID, testModel));
        }

        [Fact]
        public void UpdateModifiesAnObject()
        {
            rentalsRepository.Add(testModel, testKey);

            Assert.Equal(1, rentalsRepository.Find(ID).PreparationTimeInDays);
            Assert.Equal(1, rentalsRepository.Find(ID).Units);

            var modifiedModel = new RentalBindingModel() { PreparationTimeInDays = 2, Units = 2 };

            rentalsRepository.Update(ID, modifiedModel);

            Assert.Equal(2, rentalsRepository.Find(ID).PreparationTimeInDays);
            Assert.Equal(2, rentalsRepository.Find(ID).Units);
        }
    }
}
