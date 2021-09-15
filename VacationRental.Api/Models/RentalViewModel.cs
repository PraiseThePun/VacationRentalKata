namespace VacationRental.Api.Models
{
    public class RentalViewModel
    {
        public RentalViewModel(int id, int units, int preparationTimeInDays)
        {
            Id = id;
            Units = units;
            PreparationTimeInDays = preparationTimeInDays;
        }

        public int Id { get; }
        public int Units { get; }
        public int PreparationTimeInDays { get; }

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
