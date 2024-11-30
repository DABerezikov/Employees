namespace Employees.Common.Extensions
{
    public static class EmployeePrintExtension
    {
        private static Action<object>? _strategy;
        public static void Print(this Data.Entities.Employee? employee, Action<object>? strategy = null)
        {
            _strategy = strategy;

            if (employee == null) return;
            _strategy?.Invoke(employee);
        }
    }
}
