namespace Employees.Data.Entities
{
    public record Employee(
        int EmployeeId,
        string FirstName,
        string LastName,
        string Email,
        DateTime DateOfBirth,
        decimal Salary)
    {
        public Employee() : this(0, string.Empty, string.Empty, string.Empty, new DateTime(), 0)
        {
        }

        public override string ToString() => string.Format("| {0, 4} | {1, 15} | {2, 15} | {3, 20} | {4, 13} | {5, 20} | "
            , EmployeeId, FirstName, LastName, Email, DateOfBirth.ToShortDateString(), Salary);

    }
}
