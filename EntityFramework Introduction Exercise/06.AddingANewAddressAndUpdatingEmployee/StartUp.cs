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

            string output = AddNewAddressToEmployee(dbContext);

            Console.WriteLine(output);
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            //create new address
            Address newAddress = new Address()
            {
                
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            context.Addresses.Add(newAddress);

            var nakov = context
                .Employees
                .First(e => e.LastName == "Nakov");

            nakov.Address = newAddress;

            context.SaveChanges();

            var allAddresses = context
                .Employees
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .Select(e => e.Address.AddressText)
                .ToList();

            foreach (var address in allAddresses)
            {
                sb
                    .AppendLine(address);
            }

            return sb.ToString().TrimEnd();
        }
    }
}
