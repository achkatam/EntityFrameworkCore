namespace _4.AddMinion.IO
{
    using System;
    using _4.AddMinion.IO.Contracts;

    public class Reader : IReadable
    {
        public string ReadLine()
        => Console.ReadLine();
    }
}
