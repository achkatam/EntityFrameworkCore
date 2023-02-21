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

            string output = DeleteProjectById(dbContext);

            Console.WriteLine(output);
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            //Find the first project with Id == 2
            var projectsWithId2 = context
                .Projects
                .Find(2);

            //projects referenced to the projectsWithId2 because it is many-to-many
            var referenced = context
                .EmployeesProjects
                .Where(ep => ep.ProjectId == projectsWithId2.ProjectId)
                .ToList();

            context.EmployeesProjects.RemoveRange(referenced);

            context.Projects.Remove(projectsWithId2);

            //context.SaveChanges();

            var allProjects = context
                .Projects
                .Select(p => p.Name)
                .Take(10)
                .ToList();

            foreach (var project in allProjects)
            {
                sb
                    .AppendLine(project);
            }

            return sb.ToString().TrimEnd();
        }
    }
}