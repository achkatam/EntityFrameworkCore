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

            string output = GetLatestProjects(dbContext);

            Console.WriteLine(output);
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var projects = context
                .Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
               .OrderBy(p => p.Name)
                .ToArray();

            foreach (var project in projects)
            {
                sb
                    .AppendLine(project.Name)
                    .AppendLine(project.Description)
                    .AppendLine(project.StartDate.ToString("M/d/yyyy h:mm:ss tt"));
            }

            return sb.ToString().TrimEnd();
        }
    }
}
