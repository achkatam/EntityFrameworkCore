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
            SoftUniContext dbContext = new SoftUniContext();

            string output = GetEmployeesWithSalaryOver50000(dbContext);

            Console.WriteLine(output);
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            List<Employee> employees = context
                .Employees.Where(s => s.Salary > 50_000)
                .OrderBy(e => e.FirstName)
                .ToList();

            foreach (Employee e in employees)
            {
                sb
                    .AppendLine($"{e.FirstName} - {e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
