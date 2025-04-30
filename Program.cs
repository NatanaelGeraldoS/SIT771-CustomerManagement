using System;
using CustomerManagement.Data;
using CustomerManagement.Models;
using Microsoft.Extensions.Configuration;
using SplashKitSDK;

// dotnet add package Microsoft.Extensions.Configuration
// dotnet add package Microsoft.Extensions.Configuration.Json

namespace CustomerManagement
{
    public class Program
    {
        public static void Main()
        {
            // Get the Config from the appsettings.json
            var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
            // Take the credential from connectionstring name MySqlConnection
            string connectionString = config.GetConnectionString("MySqlConnection") ?? "";
            // Create the Connection to the table
            var CustomerRepository = new CustomerRepository(connectionString);

            //Loop until the user exit the program
            while (true)
            {
                // Show menus
                Console.WriteLine("\n1. Show all customers");
                Console.WriteLine("2. Add customer");
                Console.WriteLine("3. Update customer Data");
                Console.WriteLine("4. Delete customer");
                Console.WriteLine("0. Exit");

                var input = Console.ReadLine();
                if (input == "1")
                {
                    // Get All User information from CustomerRepository
                    var customers = CustomerRepository.GetAll();
                    // Loop and show all user information and data
                    customers.ForEach(c => Console.WriteLine($"{c.Id}: {c.Name} - {c.Email}"));
                }
                else if (input == "2")
                {
                    // Ask user the name and email for new customer
                    Console.Write("Name: ");
                    var name = Console.ReadLine() ?? "";
                    Console.Write("Email: ");
                    var email = Console.ReadLine() ?? "";
                    // Add the user to the database
                    bool isSuccess = CustomerRepository.Add(new Customer { Name = name, Email = email });
                    if (isSuccess)
                    {
                        Console.WriteLine("Customer added!");
                    }
                    else
                    {
                        Console.WriteLine("Failed to add new user information");
                    }

                }
                else if (input == "3")
                {
                    // Ask user the user detail such the Id, new name and new email
                    Console.Write("Enter ID to update: ");
                    int id = int.Parse(Console.ReadLine()!);
                    Console.Write("Change Name to: ");
                    var name = Console.ReadLine() ?? "";
                    Console.Write("Change Email to: ");
                    var email = Console.ReadLine() ?? "";
                    // Update the user information in the database
                    bool isSuccess = CustomerRepository.Update(new Customer { Id = id, Name = name, Email = email });
                    if (isSuccess)
                    {
                        Console.WriteLine("Customer deleted!");
                    }
                    else
                    {
                        Console.WriteLine("Failed to update user information");
                    }
                }
                else if (input == "4")
                {
                    // Ask the user Id to be delete
                    Console.Write("Enter ID to delete: ");
                    int id = int.Parse(Console.ReadLine()!);
                    // Delete the user information from database
                    bool isSuccess = CustomerRepository.Delete(id);
                    if (isSuccess)
                    {
                        Console.WriteLine("Customer deleted!");
                    }
                    else
                    {
                        Console.WriteLine("Failed to delete user information");
                    }
                }
                else if (input == "0")
                {
                    // Exit the program
                    Console.WriteLine("Thank You!");
                    Console.WriteLine("Natanael Geraldo Sulaiman - 225000297");
                    break;
                }
            }

        }
    }
}
