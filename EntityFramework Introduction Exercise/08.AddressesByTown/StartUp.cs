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

            string output = GetAddressesByTown(dbContext);

            Console.WriteLine(output);
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var allAddresses = context
                .Addresses
                .OrderByDescending(e => e.Employees.Count)
                .ThenBy(t => t.Town.Name)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .Select(n => new
                {
                    n.AddressText,
                    TownName = n.Town.Name,
                    EmployeesCnt = n.Employees.Count
                })
                .ToList();

            foreach (var ad in allAddresses)
            {
                sb
                    .AppendLine($"{ad.AddressText}, {ad.TownName} - {ad.EmployeesCnt} employees");
            }


            return sb.ToString().TrimEnd();
        }
    }
}
