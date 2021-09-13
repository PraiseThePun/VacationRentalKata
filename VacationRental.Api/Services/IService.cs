namespace VacationRental.Api.Services
{
    public interface IService<T, U, G>
    {
        T Find(int id);
        void Add(G model, U key);
        T Update(int id, G value);
    }
}
