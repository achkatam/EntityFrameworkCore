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

            string output = GetEmployeesByFirstNameStartingWithSa(dbContext);

            Console.WriteLine(output);
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context
                .Employees
                .Where(e => e.FirstName.StartsWith("Sa"))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .ToList();

            foreach (var e in employees)
            {
                sb
                    .AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
