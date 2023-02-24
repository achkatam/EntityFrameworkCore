using System.ComponentModel;
using System.Globalization;

namespace SoftUni
{
    using System;
    using System.Linq;
    using System.Text;
    using Data;

    public class StartUp
    {
        static void Main(string[] args)
        {

            using SoftUniContext dbContext = new SoftUniContext();

            string output = GetEmployeesInPeriod(dbContext);

            Console.WriteLine(output);
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var allEmployees = context
                .Employees
             
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    ManagerFirstName = e.Manager.FirstName,
                    ManagerLastName = e.Manager.LastName,
                    AllProjects = e.EmployeesProjects
                        .Where(e => e.Project.StartDate.Year >= 2001 && e.Project.StartDate.Year <= 2003)
                        .Select(ep => new
                        {
                            ep.Project.Name,
                            ep.Project.StartDate,
                            ep.Project.EndDate
                        })
                })
                .ToList();

            foreach (var e in allEmployees)
            {
                sb
                    .AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");

                string datePattern = "M/d/yyyy h:mm:ss tt";

                foreach (var p in e.AllProjects)
                {
                    var endDate = p.EndDate.HasValue ? p.EndDate.Value.ToString(datePattern, CultureInfo.InvariantCulture) : "not finished";

                    sb
                        .AppendLine($"--{p.Name} - {p.StartDate.ToString(datePattern, CultureInfo.InvariantCulture)} - {endDate}");
                }
            }


            return sb.ToString().TrimEnd();
        }
    }
}
