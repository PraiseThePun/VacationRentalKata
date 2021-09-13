using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Models;

namespace VacationRental.Api.Repos
{
    public class BookingsRepository : IRepository<BookingViewModel, ResourceIdViewModel, BookingBindingModel>
    {
        private readonly IDictionary<int, RentalViewModel> rentals;
        private readonly IDictionary<int, BookingViewModel> bookings;

        public BookingsRepository(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings)
        {
            this.rentals = rentals;
            this.bookings = bookings;
        }

        public void Add(BookingBindingModel model, ResourceIdViewModel key)
        {
            if (!rentals.ContainsKey(model.RentalId))
                throw new ApplicationException("Rental not found");
            Validate(model);

            var count = FindRentals(model);
            if (count >= rentals[model.RentalId].Units)
                throw new ApplicationException("Not available");

            bookings.Add(key.Id, new BookingViewModel
            {
                Id = key.Id,
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date
            });
        }

        private int FindRentals(BookingBindingModel model)
        {
            var preparationNights = rentals[model.RentalId].PreparationTimeInDays;

            if (bookings?.Count > 0)
            {
                var filteredList = bookings.Values.Where(x => x.RentalId == model.RentalId
                            && (x.Start <= model.Start.Date && x.Start.AddDays(x.Nights + preparationNights) > model.Start.Date)
                            || (x.Start < model.Start.AddDays(model.Nights + preparationNights) && x.Start.AddDays(x.Nights + preparationNights) >= model.Start.AddDays(model.Nights + preparationNights))
                            || (x.Start > model.Start && x.Start.AddDays(x.Nights + preparationNights) < model.Start.AddDays(model.Nights + preparationNights)));

                return filteredList.Count();
            }

            return 0;
        }

        public BookingViewModel Find(int id)
        {
            CheckExists(id);

            return bookings[id];
        }

        public int GetNextKey()
        {
            return bookings.Keys.Count + 1;
        }

        public BookingViewModel Update(int id, BookingBindingModel value)
        {
            CheckExists(id);

            bookings[id] = new BookingViewModel()
            {
                Id = id,
                Nights = value.Nights,
                RentalId = value.RentalId,
                Start = value.Start
            };

            return bookings[id];
        }

        private void CheckExists(int id)
        {
            if (!bookings.ContainsKey(id))
                throw new ApplicationException("Booking not found");
        }

        private void Validate(BookingBindingModel model)
        {
            if (model.Nights <= 0)
                throw new ApplicationException("Nigts must be positive");
        }
    }
}
