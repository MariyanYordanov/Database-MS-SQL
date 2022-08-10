namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;

    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.Data.Models;
    using TeisterMask.Data.Models.Enums;
    using TeisterMask.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            ICollection<Project> projects = new List<Project>();
            var dtos = XmlConverter.XmlConverter.Deserializer<ImportProjectsDto>(xmlString, "Projects");
            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!DateTime.TryParseExact(dto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validProjectOpenDate))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime? projectDate = null;
                if (!String.IsNullOrEmpty(dto.DueDate))
                {
                    if (!DateTime.TryParseExact(dto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validProjectDueDate))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    projectDate = validProjectDueDate;
                }

                Project project = new Project()
                {
                    Name = dto.Name,
                    OpenDate = validProjectOpenDate,
                    DueDate = projectDate,
                };

                foreach (var tDto in dto.Tasks)
                {
                    if (!IsValid(tDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (!DateTime.TryParseExact(tDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validTaskOpenDate))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (!DateTime.TryParseExact(tDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validTaskDueDate))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (validTaskOpenDate < validProjectOpenDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (validTaskDueDate > projectDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Task task = new Task()
                    {
                        Name = tDto.Name,
                        OpenDate = validTaskOpenDate,
                        DueDate = validTaskDueDate,
                        ExecutionType = (ExecutionType)tDto.ExecutionType,
                        LabelType = (LabelType)tDto.LabelType,
                    };

                    project.Tasks.Add(task);
                }

                projects.Add(project);
                sb.AppendLine(String.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count));
            }

            context.Projects.AddRange(projects);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            StringBuilder stringBuilder = new StringBuilder();
            List<Employee> employees = new List<Employee>();
            var dtos = JsonConvert.DeserializeObject<ImportEmployeesDto[]>(jsonString);
            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    stringBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                Employee employee = new Employee()
                {
                    Username = dto.Username,
                    Email = dto.Email,
                    Phone = dto.Phone,
                };

                foreach (int id in dto.Tasks.Distinct())
                {
                    if (!IsValid(id))
                    {
                        stringBuilder.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (context.Tasks.Find(id) == null)
                    {
                        stringBuilder.AppendLine(ErrorMessage);
                        continue;
                    }

                    EmployeeTask employeeTask = new EmployeeTask()
                    {
                        TaskId = id,
                        Employee = employee,
                    };

                    employee.EmployeesTasks.Add(employeeTask);
                }

                employees.Add(employee);
                stringBuilder.AppendLine(String
                    .Format(SuccessfullyImportedEmployee, employee.Username, employee.EmployeesTasks.Count));
            }

            context.Employees.AddRange(employees);
            context.SaveChanges();

            return stringBuilder.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}