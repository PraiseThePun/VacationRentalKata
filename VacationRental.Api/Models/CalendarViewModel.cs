using System.Collections.Generic;
using System.Linq;

namespace VacationRental.Api.Models
{
    public class CalendarViewModel
    {
        public CalendarViewModel(int rentalId, List<CalendarDateViewModel> dates)
        {
            RentalId = rentalId;
            Dates = dates;
        }

        public int RentalId { get; }
        public List<CalendarDateViewModel> Dates { get; }

        public override bool Equals(object obj)
        {
            if(obj is CalendarViewModel)
            {
                var that = obj as CalendarViewModel;

                return this.RentalId == that.RentalId
                    && this.Dates.SequenceEqual(that.Dates);
            }

            return false;
        }
    }
}
