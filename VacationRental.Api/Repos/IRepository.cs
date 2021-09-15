namespace VacationRental.Api.Repos
{
    public interface IRepository<T, U, G>
    {
        T Find(int id);
        void Add(G model, U key);
        T Update(int id, G value);

        int GetNextKey();
    }
}
