namespace VacationRental.Api.Models
{
    public class PreparationTimeViewModel
    {
        public PreparationTimeViewModel(int unit)
        {
            Unit = unit;
        }

        public int Unit { get; }

        public override bool Equals(object obj)
        {
            if(obj is PreparationTimeViewModel)
            {
                var that = obj as PreparationTimeViewModel;

                return this.Unit == that.Unit;
            }

            return false;
        }
    }
}
