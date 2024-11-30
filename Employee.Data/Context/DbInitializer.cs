using Dapper;
using Microsoft.Data.SqlClient;

namespace Employees.Data.Context;

public class DbInitializer : IDisposable
{
    public DbInitializer()
    {
        Initialize();
    }

    public SqlConnection? Connection { get; private set; }

    private const string ConnectionString = "Server=localhost;Database=master;TrustServerCertificate=True;Trusted_Connection=True;";

    private void Initialize()
    {
        Connection = new SqlConnection(ConnectionString);
        try
        {
            Connection.Open();
            //Console.WriteLine("Подключение открыто");
        }
        catch (SqlException ex)
        {
            Console.WriteLine(ex.Message);
        }

        CreateTables();

    }

    private void CreateTables()
    {
        const string sqlCreateTables = @"
                                            IF NOT EXISTS(
                                                SELECT *
                                                FROM INFORMATION_SCHEMA.TABLES
                                                WHERE TABLE_NAME = 'Employee' )
                                            BEGIN
                                            CREATE TABLE Employee (
                                            EmployeeId INT PRIMARY KEY IDENTITY (1,1),
                                            FirstName NVARCHAR(50) NOT NULL,
                                            LastName NVARCHAR(50) NULL,
                                            Email NVARCHAR(100) NULL,
                                            DateOfBirth DATE NULL,
                                            Salary FLOAT NULL
                                            )
                                            END
                                            ";
        try
        {

            Connection?.Execute(sqlCreateTables);

        }
        catch (SqlException ex)
        {
            Console.WriteLine(ex.Message);
        }


    }

    public void Dispose()
    {
        Connection?.Close();
    }
}