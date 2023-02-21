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

            string output = RemoveTown(dbContext);

            Console.WriteLine(output);
        }

        public static string RemoveTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var townToGetDeleted = context
                .Towns
                .Where(t => t.Name == "Seattle")
                .FirstOrDefault();

            //referenced addresses to the town with Seattles's Id
            var refferedAddresses = context
                .Addresses.Where(a => a.TownId == townToGetDeleted.TownId)
                .ToList();

            foreach (var e in context.Employees)
            {
                if (refferedAddresses.Any(r => r.AddressId == e.AddressId))
                {
                    e.AddressId = null;
                }
            }

            var deletedAddressesCnt = refferedAddresses.Count;

            context.Addresses.RemoveRange(refferedAddresses);
            context.Towns.Remove(townToGetDeleted);

            sb.AppendLine($"{deletedAddressesCnt} addresses in Seattle were deleted");

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
    }
}
