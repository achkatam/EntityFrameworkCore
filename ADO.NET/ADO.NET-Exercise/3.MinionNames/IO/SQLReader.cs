using ADO.NET_Exercise.IO.Contracts;

namespace ADO.NET_Exercise.IO
{

    public class SQLReader : IReader
    {
        public SQLReader(string fileName)
        {
            FileName = fileName;
        }
        public string FileName { get; }


        public string ReadLine()
        {
            string currDirectoryPath = Directory.GetCurrentDirectory();
            string fullPath = Path.Combine(currDirectoryPath, $"../Queries/{FileName}.sql");

            string query = File.ReadAllText(fullPath);

            return query;
        }
    }
}
