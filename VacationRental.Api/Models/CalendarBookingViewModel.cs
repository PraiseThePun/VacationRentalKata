namespace VacationRental.Api.Models
{
    public class CalendarBookingViewModel
    {
        public int Id { get; set; }
        public int Unit { get; set; }

        public override bool Equals(object obj)
        {
            if(obj is CalendarBookingViewModel)
            {
                var that = obj as CalendarBookingViewModel;

                return this.Id == that.Id
                    && this.Unit == that.Unit;
            }

            return false;
        }
    }
}
