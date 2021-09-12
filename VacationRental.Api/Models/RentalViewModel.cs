namespace VacationRental.Api.Models
{
    public class RentalViewModel
    {
        public int Id { get; set; }
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }

        public override bool Equals(object obj)
        {
            if(obj is RentalViewModel)
            {
                var that = obj as RentalViewModel;
                return this.Id == that.Id && this.Units == that.Units && this.PreparationTimeInDays == that.PreparationTimeInDays;
            }

            return false;
        }
    }
}
