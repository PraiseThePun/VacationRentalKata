using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Models;
using VacationRental.Api.Repos;

namespace VacationRental.Api.Services
{
    public class BookingService : IService<BookingViewModel, ResourceIdViewModel, BookingBindingModel>
    {
        private readonly BookingsRepository bookingsRepository;
        private readonly RentalService rentalService;

        public BookingService(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings)
        {
            bookingsRepository = new BookingsRepository(bookings);
            rentalService = new RentalService(rentals);
        }

        public void Add(BookingBindingModel model, ResourceIdViewModel key)
        {
            var rental = rentalService.Find(model.RentalId);
            Validate(model);

            var count = FindRentals(model);
            if (count >= rental.Units)
                throw new ApplicationException("Not available");

            bookingsRepository.Add(model, key);
        }

        public BookingViewModel Find(int id)
        {
            CheckExists(id);

            return bookingsRepository.Find(id);
        }

        public BookingViewModel Update(int id, BookingBindingModel value)
        {
            CheckExists(id);

            return bookingsRepository.Update(id, value);
        }

        public int GetNextKey()
        {
            return bookingsRepository.GetNextKey();
        }

        public IDictionary<int, BookingViewModel> GetAllBookings()
        {
            return bookingsRepository.GetAllBookings();
        }

        public IDictionary<int, BookingViewModel> GetBookingsByRentalId(int rentalId)
        {
            return bookingsRepository.FindBookingsByRentalId(rentalId);
        }

        private int FindRentals(BookingBindingModel model)
        {
            var bookings = bookingsRepository.FindBookingsByRentalId(model.RentalId);

            if (bookings?.Count > 0)
            {
                var rental = rentalService.Find(model.RentalId);
                var preparationNights = rental?.PreparationTimeInDays ?? 0;
                var blockedNights = model.Nights + preparationNights;

                var filteredList = bookings.Values.Where(x =>  x.Unit == model.Unit
                            && (x.Start <= model.Start.Date && x.Start.AddDays(x.Nights + preparationNights) > model.Start.Date)
                            || (x.Start < model.Start.AddDays(blockedNights) && x.Start.AddDays(x.Nights + preparationNights) >= model.Start.AddDays(blockedNights))
                            || (x.Start > model.Start && x.Start.AddDays(x.Nights + preparationNights) < model.Start.AddDays(blockedNights)));

                return filteredList.Count();
            }

            return 0;
        }

        private void Validate(BookingBindingModel model)
        {
            if (model.Nights <= 0)
                throw new ApplicationException("Nigts must be positive");
        }

        private void CheckExists(int id)
        {
            if (!bookingsRepository.GetAllBookings().ContainsKey(id))
                throw new ApplicationException("Booking not found");
        }
    }
}
