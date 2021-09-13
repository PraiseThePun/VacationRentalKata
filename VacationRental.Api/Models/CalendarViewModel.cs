using System.Collections.Generic;
using System.Linq;

namespace VacationRental.Api.Models
{
    public class CalendarViewModel
    {
        public int RentalId { get; set; }
        public List<CalendarDateViewModel> Dates { get; set; }

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
