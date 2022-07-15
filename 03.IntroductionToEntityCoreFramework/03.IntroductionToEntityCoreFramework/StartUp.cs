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
            var result = GetEmployeesFullInformation(dataContext);
            Console.WriteLine(result);
        }

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
    }
}
