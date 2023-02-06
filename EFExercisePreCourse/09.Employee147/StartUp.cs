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

            string output = GetEmployee147(dbContext);

            Console.WriteLine(output);
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employee147 = context
                .Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    AllProjects = e.EmployeesProjects
                        .OrderBy(ep => ep.Project.Name)
                        .Select(ep => new
                        {
                            ProjectName = ep.Project.Name
                        })
                        .ToArray()
                })
                .FirstOrDefault();

            sb
                .AppendLine($"{employee147.FirstName} {employee147.LastName} - {employee147.JobTitle}");

            foreach (var project in employee147.AllProjects)
            {
                sb
                    .AppendLine($"{project.ProjectName}");
            }


            return sb.ToString().TrimEnd();
        }
    }
}
