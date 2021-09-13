﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Models;

namespace VacationRental.Api.Repos
{
    public class BookingsRepository : IRepository<BookingViewModel, ResourceIdViewModel, BookingBindingModel>
    {
        private readonly RentalsRepository rentalsRepo;
        private readonly IDictionary<int, BookingViewModel> bookings;

        public BookingsRepository(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings)
        {
            this.rentalsRepo = new RentalsRepository(rentals);
            this.bookings = bookings;
        }

        public void Add(BookingBindingModel model, ResourceIdViewModel key)
        {
            var rental = FindRental(model.RentalId);
            Validate(model);

            var count = FindRentals(model);
            if (count >= rental.Units)
                throw new ApplicationException("Not available");

            bookings.Add(key.Id, new BookingViewModel
            {
                Id = key.Id,
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date,
                Unit = model.Unit
            });
        }

        private int FindRentals(BookingBindingModel model)
        {
            var rental = FindRental(model.RentalId);
            var preparationNights = rental?.PreparationTimeInDays ?? 0;

            if (bookings?.Count > 0)
            {
                var filteredList = bookings.Values.Where(x => (x.RentalId == model.RentalId
                            && x.Unit == model.Unit)
                            && (x.Start <= model.Start.Date && x.Start.AddDays(x.Nights + preparationNights) > model.Start.Date)
                            || (x.Start < model.Start.AddDays(model.Nights + preparationNights) && x.Start.AddDays(x.Nights + preparationNights) >= model.Start.AddDays(model.Nights + preparationNights))
                            || (x.Start > model.Start && x.Start.AddDays(x.Nights + preparationNights) < model.Start.AddDays(model.Nights + preparationNights)));

                return filteredList.Count();
            }

            return 0;
        }

        public RentalViewModel FindRental(int rentalId)
        {
            return rentalsRepo.Find(rentalId);
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

        public IDictionary<int, BookingViewModel> GetAllBookings()
        {
            return bookings;
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
