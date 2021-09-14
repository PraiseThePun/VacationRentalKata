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
            return repository.GetAllRentals().Keys.Count + 1;
        }

        private void CheckExists(int id)
        {
            if (!repository.GetAllRentals().ContainsKey(id))
                throw new ApplicationException("Rental not found");
        }
    }
}
