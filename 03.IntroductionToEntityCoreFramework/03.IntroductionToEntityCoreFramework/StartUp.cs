using SoftUni.Data;
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
            var result = GetEmployeesFromResearchAndDevelopment(dataContext);
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
                    e.Salary
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
                    e.Salary
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
        // Extract all employees from the Research and Development department.
        // Order them by salary (in ascending order), then by first name (in descending order).
        // Return only their first name, last name, department name and salary rounded to 2 symbols,
        // after the decimal separator in the format shown below: 
        // Gigi Matthew from Research and Development - $40900.00
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context.Employees
                .Select(e => new
                {
                    e.DepartmentId,
                    e.FirstName,
                    e.LastName,
                    e.Salary,
                    e.Departments
                })
                .Where(e => e.DepartmentId == 6)
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName);
                
            var sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} from Research and Development - ${employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
