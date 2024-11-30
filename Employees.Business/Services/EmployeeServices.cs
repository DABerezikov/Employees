using System.Collections.Immutable;
using System.Globalization;
using System.Text.RegularExpressions;
using Employees.Common.Extensions;
using Employees.Common.Interfaces;
using Employees.Data.Entities;
using Employees.Data.Repositories.Interfaces;

namespace Employees.Business.Services
{
    public class EmployeeService(IRepository<Employee> repository) : IEmployeeService
    {
        

        public void Run()
        {
            Init();
            ViewMenu();

            while (_isProcess)
            {
                Console.Write("Введите номер операции: ");
                var operation = Console.ReadLine();
                if (operation != null) Process(operation);
            }
        }
        private void Process(string operation)
        {
            switch (operation)
            {
                case "1":
                    AddNewEmployee();
                    break;
                case "2":
                    PrintEmployees();
                    break;
                case "3":
                    UpdateEmployee();
                    break;
                case "4":
                    DeleteEmployee();
                    break;
                case "5":
                    Exit();
                    break;
                default:
                    Console.WriteLine("Не верный номер операции!");
                    break;
            }
        }
        #region Helper Methods

        private void AddNewEmployee()
        {
            var newEmployee = GetEmployeeInfoAndCreate();
            _employees = _employees!.Add(newEmployee);
            _repository!.Create(newEmployee);
            AddEmployeeDone(newEmployee);
        }
        private void PrintEmployees()
        {
            PrintTitle();
            _employees!.Print(Console.WriteLine);
            Console.WriteLine();
        }

        private void UpdateEmployee()
        {
            var id = GetId();

            var updatedEmployee = _employees!.First(e => e.EmployeeId == id);


            if (!ConfirmationRequest(id, "обновить"))
            {
                ViewMenu();
                return;
            }

            GetNewEmployeeInfo(updatedEmployee);

            var updateEmployee = updatedEmployee with
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                DateOfBirth = dateOfBirth,
                Salary = salary
            };

            _employees = _employees!.Replace(updatedEmployee, updateEmployee);
            _repository!.Update(updateEmployee);

            OperationInfo(updateEmployee, "обновлен");

            ViewOperation();
        }
        private void DeleteEmployee()
        {
            var id = GetId();
            if (!ConfirmationRequest(id, "удалить"))
            {
                ViewMenu();
                return;
            }

            var deletedEmployee = _employees!.First(e => e.EmployeeId == id);

            _employees = _employees!.Remove(deletedEmployee);
            _repository!.Delete(id);

            OperationInfo(deletedEmployee, "удален");

            ViewOperation();
        }

        private void Exit()
        {
            Console.Clear();
            Console.WriteLine("Всего доброго!");
            _isProcess = false;
        }

        #region Service Methods

        private void Init()
        {
            _employees = (_repository!.GetAll() ?? []).ToImmutableList();

            if (_employees != null)
            {
                _isProcess = true;
            }

            else
            {
                Console.WriteLine("Недостаточно данных для работы!");
            }
            
        }

        private static void ViewMenu()
        {
            Console.Clear();
            ViewTitle();
            ViewOperation();
        }

        private static void ViewTitle()
        {
            const string menu = "База данных сотрудников \n";

            Console.WriteLine(menu);
        }

        private static void ViewOperation()
        {
            const string operation = """
                                 
                                 Список операций
                                     
                                 1 Добавить нового сотрудника
                                 2 Посмотреть всех сотрудников
                                 3 Обновить информацию о сотруднике
                                 4 Удалить сотрудника
                                 5 Выйти из программы
                                              
                                 """;

            Console.WriteLine(operation);
        }

        private static void ViewUpdateOperation()
        {
            const string operation = """
                                 
                                     
                                 1 Обновить фамилию сотрудника
                                 2 Обновить имя сотрудника
                                 3 Обновить Email сотрудника
                                 4 Обновить дерь рождения сотрудника
                                 5 Обновить зарплату сотрудника
                                 6 Ничего не обновлять
                                              
                                 """;
            Console.Clear();
            Console.WriteLine(operation);
        }

        private static void PrintTitle()
        {
            var str = string.Format("| {0, 4} | {1, 15} | {2, 15} | {3, 20} | {4, 13} | {5, 20} | "
                , "id", "Фамилия", "Имя", "Email", "День роджения", "Зарплата");
            Console.WriteLine(str);
            Console.WriteLine();
        }

        private void GetNewEmployeeInfo(Employee updatedEmployee)
        {
            firstName = GetInfoFromEmployee(updatedEmployee, out lastName, out email, out dateOfBirth, out salary);

            updatedEmployee.Print(Console.WriteLine);

            _isUpdateProcess = true;

            while (_isUpdateProcess)
            {
                ViewUpdateOperation();
                Console.Write("\nВведите номер операции: ");
                var operation = Console.ReadLine();
                if (operation != null) UpdateProcess(operation);

                if (!ConfirmationRequest(updatedEmployee.EmployeeId, "", "Требуется обновить еще что-то? (да/нет): "))
                {
                    _isUpdateProcess = false;
                }

            }
        }

        private void UpdateProcess(string operation)
        {
            switch (operation)
            {
                case "1":
                    firstName = GetName("фамилию");
                    break;
                case "2":
                    lastName = GetName("имя");
                    break;
                case "3":
                    email = GetEmail();
                    break;
                case "4":
                    dateOfBirth = GetDate();
                    break;
                case "5":
                    salary = GetSalary();
                    break;
                case "6":
                    _isUpdateProcess = false;
                    break;
                default:
                    Console.WriteLine("Не верный номер операции!");
                    break;
            }
        }

        private static string GetInfoFromEmployee(Employee updatedEmployee, out string lastName, out string email,
            out DateTime dateOfBirth, out decimal salary)
        {
            var firstName = updatedEmployee.FirstName;
            lastName = updatedEmployee.LastName;
            email = updatedEmployee.Email;
            dateOfBirth = updatedEmployee.DateOfBirth;
            salary = updatedEmployee.Salary;
            return firstName;
        }

        private static void OperationInfo(Employee employee, string operation)
        {
            Console.Clear();
            ViewTitle();
            Console.WriteLine("Сотрудник:\n");
            PrintTitle();
            employee.Print(Console.WriteLine);
            Console.WriteLine($"\n{operation}!");
        }

        private static bool ConfirmationRequest(int id, string operation, string message = "\nВы уверены что хотите {0} сотрудника № {1}? (да/нет): ")
        {
            bool? isConfirm = null;

            while (true)
            {
                Console.Write(message, operation, id);
                var input = Console.ReadLine();

                if (input == null) continue;

                switch (input.ToLower())
                {
                    case "да":
                        isConfirm = true;
                        break;
                    case "нет":
                        isConfirm = false;
                        break;
                    default:
                        Console.WriteLine("Формат ввода не соответствует");
                        break;

                }

                if (isConfirm == null) continue;
                break;
            }

            return (bool)isConfirm;
        }

        private int GetId()
        {
            int id;

            while (true)
            {
                Console.Write("\nВведите id сотрудника, для выполнения операции: ");
                var input = Console.ReadLine();
                if (!int.TryParse(input, out id))
                {
                    Console.WriteLine("Формат ввода не соответствует id");
                    continue;
                }

                if (_employees!.All(e => e.EmployeeId != id))
                {
                    Console.WriteLine("Такого id не существует");
                    continue;
                }
                break;
            }

            return id;

        }

        private static void AddEmployeeDone(Employee newEmployee)
        {

            OperationInfo(newEmployee, "добавлен");

            ViewOperation();
        }

        private Employee GetEmployeeInfoAndCreate()
        {
            var firstName = GetEmployeeInfo(out var lastName, out var email, out var dateOfBirth, out var salary);

            return CreateEmployee(firstName, lastName, email, dateOfBirth, salary);
        }

        private static string GetEmployeeInfo(out string lastName, out string email, out DateTime dateOfBirth,
            out decimal salary)
        {
            Console.Clear();
            var firstName = GetName("фамилию");
            lastName = GetName("имя");
            email = GetEmail();
            dateOfBirth = GetDate();
            salary = GetSalary();
            return firstName;
        }

        private Employee CreateEmployee(string firstName, string lastName, string email, DateTime dateOfBirth, decimal salary)
        {
            var newEmployee = new Employee
            {
                EmployeeId = (_employees!.MaxBy(e => e.EmployeeId)?.EmployeeId ?? 0) + 1,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                DateOfBirth = dateOfBirth,
                Salary = salary

            };
            return newEmployee;
        }

        private static decimal GetSalary()
        {
            decimal salary;

            while (true)
            {
                Console.Write("Введите введите заработную плату сотрудника: ");
                var input = Console.ReadLine();
                if (!decimal.TryParse(input, out salary))
                {
                    Console.WriteLine("Формат ввода не соответствует заработной плате");
                    continue;
                }
                break;
            }

            return salary;

        }

        private static DateTime GetDate()
        {

            DateTime date;
            var minDate = new DateTime(1900, 01, 01);
            var maxDate = new DateTime(2100, 01, 01);

            while (true)
            {
                Console.Write("Введите дату рождения в формате дд.ММ.гггг (день.месяц.год): ");
                var input = Console.ReadLine();

                if (!DateTime.TryParseExact(input, "dd.MM.yyyy", null, DateTimeStyles.None, out date))
                {
                    Console.WriteLine("Формат ввода не соответствует дате рождения");
                    continue;
                }

                if (date < minDate || date > maxDate)
                {
                    Console.WriteLine("Дата рождения вне допустимого диапазона с 1900 по 2100 годы.");
                    continue;
                }

                break;
            }

            return date;

        }

        private static string GetEmail()
        {
            string? email;
            while (true)
            {
                Console.Write("Введите Email сотрудника: ");
                email = Console.ReadLine();

                const string pattern = @"[.\-_a-z0-9]+@([a-z0-9][\-a-z0-9]+\.)+[a-z]{2,6}";
                if (email != null)
                {
                    var isMatch = Regex.Match(email, pattern, RegexOptions.IgnoreCase);

                    if (!isMatch.Success)
                    {
                        Console.WriteLine("Формат ввода не соответствует Email");
                        continue;
                    }
                }

                break;
            }

            return email!;
        }

        private static string GetName(string typeName)
        {
            string? name;
            while (true)
            {
                Console.Write($"Введите {typeName} сотрудника: ");
                name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name)) continue;

                break;
            }

            return name;
        }
        #endregion

        #endregion

        #region Fields

        private readonly IRepository<Employee>? _repository = repository;
        private bool _isProcess;
        private bool _isUpdateProcess;
        private string firstName, lastName, email;
        private DateTime dateOfBirth;
        private decimal salary;
        private ImmutableList<Employee>? _employees;
        #endregion
    }
}
