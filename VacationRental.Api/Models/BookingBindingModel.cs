using System;

namespace VacationRental.Api.Models
{
    public class BookingBindingModel
    {
        public int RentalId { get; set; }

        public DateTime Start
        {
            get => _startIgnoreTime;
            set => _startIgnoreTime = value.Date;
        }

        private DateTime _startIgnoreTime;
        public int Nights { get; set; }

        public int Unit { get; set; }

        public override bool Equals(object obj)
        {
            if(obj is BookingBindingModel)
            {
                var that = obj as BookingBindingModel;

                return this.RentalId == that.RentalId
                    && this.Start == that.Start
                    && this.Nights == that.Nights
                    && this.Unit == that.Unit;
            }

            return false;
        }
    }
}
