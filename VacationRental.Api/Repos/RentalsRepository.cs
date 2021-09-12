using System;
using System.Collections.Generic;
using VacationRental.Api.Models;

namespace VacationRental.Api.Repos
{
    public class RentalsRepository : IRepository<RentalViewModel, ResourceIdViewModel, RentalBindingModel>
    {
        private readonly IDictionary<int, RentalViewModel> rentals;

        public RentalsRepository(IDictionary<int, RentalViewModel> rentals)
        {
            this.rentals = rentals;
        }

        public void Add(RentalBindingModel model, ResourceIdViewModel key)
        {
            rentals.Add(key.Id, new RentalViewModel
            {
                Id = key.Id,
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays
            });
        }

        public RentalViewModel Find(int id)
        {
            CheckExists(id);

            return rentals[id];
        }

        public RentalViewModel Update(int id, RentalBindingModel value)
        {
            CheckExists(id);

            rentals[id] = new RentalViewModel()
            {
                Id = id,
                Units = value.Units,
                PreparationTimeInDays = value.PreparationTimeInDays
            };

            return rentals[id];
        }

        public int GetNextKey()
        {
            return rentals.Keys.Count + 1;
        }

        private void CheckExists(int id)
        {
            if (!rentals.ContainsKey(id))
                throw new ApplicationException("Rental not found");
        }
    }
}
