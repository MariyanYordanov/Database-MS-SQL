namespace TeisterMask.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using System;
    using System.Globalization;
    using System.Linq;
    using TeisterMask.DataProcessor.ExportDto;
    using TeisterMask.XmlConverter;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            var projects = context.Projects
                .Where(p => p.Tasks.Any())
                .ToArray()
                .Select(p => new ExportProjectsDto()
                {
                    TasksCount = p.Tasks.Count(),
                    ProjectName = p.Name,
                    HasEndDate = p.DueDate.HasValue ? "Yes" : "No",
                    Tasks = p.Tasks
                        .Select(t => new ExportProjectTasksDto()
                        {
                            Name = t.Name,
                            Label = t.LabelType.ToString(),
                        })
                        .OrderBy(t => t.Name)
                        .ToArray(),
                })
                .OrderByDescending(p => p.TasksCount)
                .ThenBy(p => p.ProjectName)
                .ToArray();

            return XmlConverter.Serialize(projects, "Projects");
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var employee = context.Employees
                .Where(e => e.EmployeesTasks
                    .Any(e => e.Task.OpenDate >= date))
                .ToArray()
                .Select(e => new
                {
                    e.Username,
                    Tasks = e.EmployeesTasks
                        .Where(et => et.Task.OpenDate >= date)
                        .Select(et => new
                        {
                            TaskName = et.Task.Name,
                            OpenDate = et.Task.OpenDate
                                .ToString("d", CultureInfo.InvariantCulture),
                            DueDate = et.Task.DueDate
                                .ToString("d", CultureInfo.InvariantCulture),
                            LabelType = et.Task.LabelType.ToString(),
                            ExecutionType = et.Task.ExecutionType.ToString(),
                        })
                        .OrderByDescending(et => DateTime.Parse(et.DueDate, CultureInfo.InvariantCulture))
                        .ThenBy(et => et.TaskName)
                        .ToArray(),
                })
                .OrderByDescending(e => e.Tasks.Count())
                .ThenBy(e => e.Username)
                .Take(10)
                .ToArray();

            return JsonConvert.SerializeObject(employee, Formatting.Indented);
        }
    }
}