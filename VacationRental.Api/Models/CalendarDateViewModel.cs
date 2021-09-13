using System;
using System.Collections.Generic;
using System.Linq;

namespace VacationRental.Api.Models
{
    public class CalendarDateViewModel
    {
        public DateTime Date { get; set; }
        public List<CalendarBookingViewModel> Bookings { get; set; }
        public List<PreparationTimeViewModel> PreparationTimes { get; set; }

        public override bool Equals(object obj)
        {
            if(obj is CalendarDateViewModel)
            {
                var that = obj as CalendarDateViewModel;

                return this.Date == that.Date
                    && Bookings.SequenceEqual(that.Bookings)
                    && PreparationTimes.SequenceEqual(that.PreparationTimes);
            }

            return false;
        }
    }
}
