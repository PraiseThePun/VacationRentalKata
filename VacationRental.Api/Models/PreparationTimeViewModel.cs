namespace VacationRental.Api.Models
{
    public class PreparationTimeViewModel
    {
        public int Unit { get; set; }

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
