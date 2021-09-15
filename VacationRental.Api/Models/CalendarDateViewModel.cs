using System;
using System.Collections.Generic;
using System.Linq;

namespace VacationRental.Api.Models
{
    public class CalendarDateViewModel
    {
        public CalendarDateViewModel(DateTime date, List<CalendarBookingViewModel> bookings, List<PreparationTimeViewModel> preparationTimes)
        {
            Date = date;
            Bookings = bookings;
            PreparationTimes = preparationTimes;
        }

        public DateTime Date { get; }
        public List<CalendarBookingViewModel> Bookings { get; }
        public List<PreparationTimeViewModel> PreparationTimes { get; }

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
