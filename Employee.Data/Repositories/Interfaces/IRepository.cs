using Employees.Data.Entities;

namespace Employees.Data.Repositories.Interfaces
{
    public interface IRepository<in T> where T : class
    {
        int? Create(T entity);
        int? Delete(int id);
        Employee? Get(int id);
        IEnumerable<Employee>? GetAll();
        int? Update(T entity);
    }
}
