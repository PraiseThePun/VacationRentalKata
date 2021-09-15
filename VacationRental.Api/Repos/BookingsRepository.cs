using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Models;

namespace VacationRental.Api.Repos
{
    public class BookingsRepository : IRepository<BookingViewModel, ResourceIdViewModel, BookingBindingModel>
    {
        private readonly IDictionary<int, BookingViewModel> bookings;

        public BookingsRepository(IDictionary<int, BookingViewModel> bookings)
        {
            this.bookings = bookings;
        }

        public void Add(BookingBindingModel model, ResourceIdViewModel key)
        {
            bookings.Add(key.Id, new BookingViewModel(key.Id, model.RentalId, model.Start.Date, model.Nights, model.Unit));
        }

        public BookingViewModel Find(int id)
        {
            return bookings[id];
        }

        public IDictionary<int, BookingViewModel> FindBookingsByRentalId(int rentalId)
        {
            return bookings.Where(x => x.Value.RentalId == rentalId).ToDictionary(x => x.Key, x => x.Value);
        }

        public BookingViewModel Update(int id, BookingBindingModel value)
        {
            bookings[id] = new BookingViewModel(id, value.RentalId, value.Start, value.Nights, value.Unit);

            return bookings[id];
        }

        public IDictionary<int, BookingViewModel> GetAllBookings()
        {
            return bookings;
        }

        public int GetNextKey()
        {
            return bookings.Keys.Count + 1;
        }
    }
}
