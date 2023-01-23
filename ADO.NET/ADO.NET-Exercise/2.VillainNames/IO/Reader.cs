namespace ADO.NET_Exercise.IO.Contracts
{
    using System;
    
    public class Reader : IReader
    {
        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
