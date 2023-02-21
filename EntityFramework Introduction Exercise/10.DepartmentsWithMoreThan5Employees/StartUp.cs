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

            string output = GetDepartmentsWithMoreThan5Employees(dbContext);

            Console.WriteLine(output);
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var depts = context
                .Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    d.Name,
                    ManagerFN = d.Manager.FirstName,
                    ManagarLN = d.Manager.LastName,
                    Employees = d.Employees
                        .OrderBy(e => e.FirstName)
                        .ThenBy(e => e.LastName)
                        .Select(e => new
                        {
                            e.FirstName,
                            e.LastName,
                            e.JobTitle
                        })
                        .ToArray()
                })
                .ToArray();

            foreach (var dept in depts)
            {
                sb
                    .AppendLine($"{dept.Name} - {dept.ManagerFN} {dept.ManagarLN}");

                foreach (var e in dept.Employees)
                {
                    sb
                        .AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}
