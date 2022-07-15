using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Linq;
using System.Text;

namespace SoftUni

{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var dataContext = new SoftUniContext();
            var result = AddNewAddressToEmployee(dataContext);
            Console.WriteLine(result);
        }

        // 3.Employees Full Information
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var emploeeInfo = context.Employees
                .Select(e => new 
                {
                    e.EmployeeId,
                    e.FirstName,
                    e.MiddleName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary,
                })
                .OrderBy(e => e.EmployeeId)
                .ToList();
            //Guy Gilbert R Production Technician 12500.00
            StringBuilder sb = new StringBuilder();
            foreach (var employee in emploeeInfo)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:f2}");
            }

            var result = sb.ToString().TrimEnd();
            
            return result;
        }

        // 4.Employees with Salary Over 50 000
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary,
                })
                .Where(e => e.Salary > 50000)
                .OrderBy(e => e.FirstName)
                .ToList();
            var sb = new StringBuilder();
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        // 5.Employees from Research and Development
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Salary,
                    DepartmentName = e.Department.Name,
                })
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName);

            var sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.DepartmentName} - ${employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        // 6.Adding a New Address and Updating Employee
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            Employee nakov = context.Employees
                .FirstOrDefault(e => e.LastName == "Nakov");

            if (nakov != null) 
            {
                nakov.Address = new Address
                {
                    AddressText = "Vitoshka 15",
                    TownId = 4,
                };
            }

            context.SaveChanges();

            var addresses = context.Employees
                .Select(e => new 
                { 
                    e.Address.AddressText, 
                    e.Address.AddressId, 
                })
                .OrderByDescending(e => e.AddressId)
                .Take(10);

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var currentAddress in addresses)
            {
                stringBuilder.AppendLine($"{currentAddress.AddressText}");
            }

            return stringBuilder.ToString().TrimEnd();
        }
    }
}
