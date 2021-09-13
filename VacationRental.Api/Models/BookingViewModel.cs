using System;

namespace VacationRental.Api.Models
{
    public class BookingViewModel
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }

        public override bool Equals(object obj)
        {
            if(obj is BookingViewModel)
            {
                var that = obj as BookingViewModel;
                return this.Id == that.Id
                    && this.RentalId == that.RentalId
                    && this.Start == that.Start
                    && this.Nights == this.Nights;
            }

            return false;
        }
    }
}
