using System;
using System.Collections.Generic;
using VacationRental.Api.Models;
using VacationRental.Api.Repos;

namespace VacationRental.Api.Services
{
    public class RentalService : IService<RentalViewModel, ResourceIdViewModel, RentalBindingModel>
    {
        private readonly RentalsRepository repository;

        public RentalService(IDictionary<int, RentalViewModel> rentals)
        {
            repository = new RentalsRepository(rentals);
        }

        public void Add(RentalBindingModel model, ResourceIdViewModel key)
        {
            Validate(model);

            repository.Add(model, key);
        }

        public RentalViewModel Find(int id)
        {
            CheckExists(id);

            return repository.Find(id);
        }

        public RentalViewModel Update(int id, RentalBindingModel value)
        {
            CheckExists(id);

            return repository.Update(id, value);
        }

        public int GetNextKey()
        {
            return repository.GetNextKey();
        }

        private void CheckExists(int id)
        {
            if (!repository.GetAllRentals().ContainsKey(id))
                throw new ApplicationException("Rental not found");
        }

        private void Validate(RentalBindingModel model)
        {
            if(model.Units < 0)
                throw new ApplicationException("Units must be bigger than 0");

            if (model.PreparationTimeInDays <= 0)
                throw new ApplicationException("Preparation time must be a positive integer.");
        }
    }
}
