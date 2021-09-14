namespace VacationRental.Api.Models
{
    public class CalendarBookingViewModel
    {
        public CalendarBookingViewModel(int id, int unit)
        {
            Id = id;
            Unit = unit;
        }

        public int Id { get; }
        public int Unit { get; }

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
