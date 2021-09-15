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
            rentals.Add(key.Id, new RentalViewModel(key.Id, model.Units, model.PreparationTimeInDays));
        }

        public RentalViewModel Find(int id)
        {
            return rentals[id];
        }

        public RentalViewModel Update(int id, RentalBindingModel value)
        {
            rentals[id] = new RentalViewModel(id, value.Units, value.PreparationTimeInDays);

            return rentals[id];
        }

        public IDictionary<int, RentalViewModel> GetAllRentals()
        {
            return rentals;
        }

        public int GetNextKey()
        {
            return rentals.Keys.Count + 1;
        }
    }
}
