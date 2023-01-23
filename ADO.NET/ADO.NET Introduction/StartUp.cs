using System;
using System.Data.SqlClient;
using ASP.NETConnectionToSQLServerInDocker;

namespace ASPNETConnectionToSQLServerInDocker
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            //1.Install DataProvider for ADO.NET
            //2.DataProvider for SQL Server -> SqlClint
            //3.Auhtentication 
            //4.SqlCredential sqlCredential = new SqlCredential();
            //5.Create connection
            //6."using" for SqlConnection
            using SqlConnection sqlConnection =
                 new SqlConnection(Config.ConnectionString);

            //7.Open connection
            sqlConnection.Open();
            //if attemptin CRUD use Transanctions
            //SqlTransaction transaction = sqlConnection.BeginTransaction();

            //create query
            string employeeCntQry = @"SELECT COUNT(*) AS [EmployeeCount]" +
                            "FROM [Employees]";

            //for transaction
            //SqlCommand sqlCommand = new SqlCommand(query, sqlConnection, transaction);

            //create the command
            SqlCommand employeeCntCommand = new SqlCommand(employeeCntQry, sqlConnection);

            //Types of executing commands
            //ExecuteScalar() --> SELECT, ONE Row, ONE Col
            //ExecuteReader() --> SELECT,READ, Many Rows, Many Columns
            //ExecuteNonQuery() CUD Operations -> CREATE, INSERT, DELETE, UPDATE and etc.

            int employeeCnt = (int)employeeCntCommand.ExecuteScalar();
            Console.WriteLine($"Available employees: {employeeCnt}");

            //add parameters for protection from SQL injection
            string jobTitleEnt = Console.ReadLine();

            string employeeInfoQuery = @$"SELECT 
                                    	e.FirstName,
                                    	e.LastName,
                                    	e.JobTitle
                                    FROM Employees AS e
                                    WHERE e.JobTitle = @jobTitle";

            SqlCommand employeeInfoCmd = new SqlCommand(employeeInfoQuery, sqlConnection);
            employeeInfoCmd.Parameters.AddWithValue("@jobTitle", jobTitleEnt);

            using SqlDataReader employeeInfoReader =
                             employeeInfoCmd.ExecuteReader();
            int cnt = 1;

            while (employeeInfoReader.Read())
            {
                string firstName = (string)employeeInfoReader["FirstName"];
                string lastName = (string)employeeInfoReader["LastName"];
                string jobTitle = (string)employeeInfoReader["JobTitle"];

                Console.WriteLine($"#{cnt++}. {firstName} {lastName} - {jobTitle}");
            }


            //Doesn't matter the connections will get closed automatically, ALWAYS make sure you close them yourself
            employeeInfoReader.Close();
            sqlConnection.Close();

            Console.WriteLine("---------------------------");
            //Automatically closes the connection
            Console.WriteLine("Connected successfully!");
        }
    }
}