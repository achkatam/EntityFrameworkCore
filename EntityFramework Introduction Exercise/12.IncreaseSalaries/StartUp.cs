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

            string output = IncreaseSalaries(dbContext);

            Console.WriteLine(output);
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            string[] depts = new string[]
            {
                "Engineering",
                "Tool Design",
                "Marketing",
                "Information Services"
            };

            var employees = context
                .Employees
                .Where(e => depts.Contains(e.Department.Name))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToArray();


            foreach (var employee in employees)
            {
                employee.Salary *= 1.12M;
            }

            context.SaveChanges();

            foreach (var e in employees)
            {
                sb
                    .AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
