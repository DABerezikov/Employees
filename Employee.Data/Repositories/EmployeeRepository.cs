using Dapper;
using Employees.Data.Context;
using Employees.Data.Entities;
using Employees.Data.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace Employees.Data.Repositories;

public class EmployeeRepository(DbInitializer dbInitializer) : IRepository<Employee>
{

    private SqlConnection? Connection { get; } = dbInitializer.Connection;

    public int? Create(Employee entity)
    {
        const string sqlquery = """
                                INSERT INTO Employee (FirstName, LastName, Email, DateOfBirth, Salary)
                                VALUES (@firstName, @lastName, @email, @dateOfBirth, @salary)
                                """;
        int? result = null;

        try
        {
            result = Connection?.Execute(sqlquery
                , new
                {
                    firstName = entity.FirstName,
                    lastName = entity.LastName,
                    email = entity.Email,
                    dateOfBirth = entity.DateOfBirth,
                    salary = entity.Salary
                });
        }

        catch (Exception ex)
        {
            Console.WriteLine($"При создании сотрудника возникло исключение: \n{ex.Message}");
        }

        return result;
    }

    public int? Delete(int id)
    {
        const string sqlquery = """
                                DELETE FROM Employee
                                WHERE EmployeeId = @id
                                """;
        int? result = null;

        try
        {
            result = Connection?.Execute(sqlquery, new { id });
        }

        catch (Exception ex)
        {
            Console.WriteLine($"При удалении сотрудника возникло исключение: \n{ex.Message}");
        }

        return result;
    }

    public Employee? Get(int id)
    {
        const string sqlquery = """
                                SELECT  EmployeeId,  
                                    FirstName, 
                                    LastName,
                                    Email, 
                                    DateOfBirth, 
                                    Salary                                    
                                FROM Employee
                                WHERE EmployeeId = @id
                                """;
        Employee? result = null;

        try
        {
            Connection?.QueryFirst<Employee>(sqlquery, new { id });
        }

        catch (Exception ex)
        {
            Console.WriteLine($"При получении сотрудника возникло исключение: \n{ex.Message}");
        }

        return result;
    }

    public IEnumerable<Employee>? GetAll()
    {
        const string sqlquery = """
                                SELECT  EmployeeId,  
                                    FirstName, 
                                    LastName,
                                    Email, 
                                    DateOfBirth, 
                                    Salary                                    
                                FROM Employee
                                """;

        IEnumerable<Employee>? result = null;

        try
        {
            result = Connection?.Query<Employee>(sqlquery);
        }

        catch (Exception ex)
        {
            Console.WriteLine($"При получении списка сотрудников возникло исключение: \n{ex.Message}");
        }

        return result;
    }

    public int? Update(Employee entity)
    {
        const string sqlquery = """
                                UPDATE Employee
                                SET FirstName = @firstName,
                                    LastName = @lastName, 
                                    Email = @email,
                                    DateOfBirth = @dateOfBirth,
                                    Salary = @salary
                                WHERE EmployeeId = @id
                                """;
        int? result = null;

        try
        {
            Connection?.Execute(sqlquery
                , new
                {
                    id = entity.EmployeeId,
                    firstName = entity.FirstName,
                    lastName = entity.LastName,
                    email = entity.Email,
                    dateOfBirth = entity.DateOfBirth,
                    salary = entity.Salary
                });
        }

        catch (Exception ex)
        {
            Console.WriteLine($"При обновлении сотрудника возникло исключение: \n{ex.Message}");
        }

        return result;
    }
}