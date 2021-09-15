using System;

namespace VacationRental.Api.Models
{
    public class BookingViewModel
    {
        public BookingViewModel(int id, int rentalId, DateTime start, int nights, int unit)
        {
            this.Id = id;
            this.RentalId = rentalId;
            this.Start = start;
            this.Nights = nights;
            this.Unit = unit;
        }

        public int Id { get; }
        public int RentalId { get; }
        public DateTime Start { get; }
        public int Nights { get; }
        public int Unit { get; }

        public override bool Equals(object obj)
        {
            if(obj is BookingViewModel)
            {
                var that = obj as BookingViewModel;
                return this.Id == that.Id
                    && this.RentalId == that.RentalId
                    && this.Start == that.Start
                    && this.Nights == that.Nights
                    && this.Unit == that.Unit;
            }

            return false;
        }
    }
}
