using System.Collections.Generic;
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
            bookings.Add(key.Id, new BookingViewModel
            {
                Id = key.Id,
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date,
                Unit = model.Unit
            });
        }

        public RentalViewModel FindRental(int rentalId)
        {
            return rentalsRepo.Find(rentalId);
        }

        public BookingViewModel Find(int id)
        {
            return bookings[id];
        }

        public int GetNextKey()
        {
            return bookings.Keys.Count + 1;
        }

        public BookingViewModel Update(int id, BookingBindingModel value)
        {
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
    }
}
