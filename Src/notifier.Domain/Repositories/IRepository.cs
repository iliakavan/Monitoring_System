namespace notifier.Domain.Repositories;



public interface IRepository<T> where T : class 
{
    Task Add(T model);

    void Update(T model);

    void Delete(T model);

    Task<T?> GetById(int id);

    Task<IEnumerable<T>> GetAll();

}
