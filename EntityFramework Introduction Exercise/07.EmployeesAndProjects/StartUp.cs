using System.Transactions;

namespace SoftUni
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Data;
    using Models;

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
                .Where(e => e.EmployeesProjects
                    .Any(ep => ep.Project.StartDate.Year >= 2001
                               && ep.Project.StartDate.Year <= 2003))
                .Take(10)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    ManagerFirstName = e.Manager.FirstName,
                    ManagerLastName = e.Manager.LastName,
                    AllProjects = e.EmployeesProjects
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

                foreach (var p in e.AllProjects)
                {
                    var endDate = p.EndDate.HasValue ? p.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt") : "not finished";

                    sb
                        .AppendLine($"--{p.Name} - {p.StartDate} - {endDate}");
                }
            }


            return sb.ToString().TrimEnd();
        }
    }
}
