using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CatWorx.BadgeMaker
{
    class PeopleFetcher
    {
        public static List<Employee> GetEmployees()
        {
            List<Employee> employees = new List<Employee>();
            // Collect user values until the value is an empty string
            while (true)
            {
                // Move the initial prompt inside the loop, so it repeasts for each employee
                Console.WriteLine("Enter first name: (leave empty to exit): ");

                string firstName = Console.ReadLine() ?? "";
                if (firstName == "")
                {
                    break;
                }

                //  add Console.ReadLine() for each value
                Console.Write("Enter a last name: ");
                string lastName = Console.ReadLine() ?? "";
                Console.Write("Enter ID: ");
                int id = Int32.Parse(Console.ReadLine() ?? "");
                Console.Write("Enter Photo URL: ");
                string photoUrl = Console.ReadLine() ?? "";
                Employee currentEmployee = new Employee(firstName, lastName, id, photoUrl);
                // Add currentEmployee, not a string
                employees.Add(currentEmployee);
            }
            return employees;
        }

        async public static Task<List<Employee>> GetFromApi()
        {
            List<Employee> employees = new List<Employee>();
            using (HttpClient client = new HttpClient())
            {
                string response = await client.GetStringAsync("https://randomuser.me/api/?results=10&nat=us&inc=name,id,picture");
                JObject json = JObject.Parse(response);
                foreach (JToken token in json.SelectToken("results")!)
                {
                    // Parse JSON data
                    Employee emp = new Employee
                    (
                      token.SelectToken("name.first")!.ToString(),
                      token.SelectToken("name.last")!.ToString(),
                      Int32.Parse(token.SelectToken("id.value")!.ToString().Replace("-", "")),
                      token.SelectToken("picture.large")!.ToString()
                    );
                    employees.Add(emp);
                }
            }
            return employees;

        }
    }
}