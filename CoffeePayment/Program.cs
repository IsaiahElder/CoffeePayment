using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;
using Newtonsoft;

internal class Program
{
    private static void Main(string[] args)
    {
        bool ordering = true;
        while (ordering)
        {
            Console.Clear();
            Directory.CreateDirectory("C:\\CoffeePayments");
            string path = Path.GetFullPath("C:\\CoffeePayments\\CoffeeCustomers.json");
            List<Employee> employees = extractEmployees(path);

            Console.WriteLine(" - - - - - - - - Coffee bean - - - - - - - - - ");
            Console.WriteLine("Did everyone order their usual this time? [y/n]");

            double totalPayment = actOnResponse(employees);
            string paysToday = whoShouldPay(employees);

            Console.WriteLine($"{paysToday} should pay today");

            updateTotalPayments(employees, totalPayment, paysToday);
            updateJSON(path, employees);
            ordering = orderAgainQuerey(ordering);
        }
    }
    /// <summary>
    /// Ask the user if they would like to order again.
    /// </summary>
    /// <param name="ordering">Current ordering status</param>
    /// <returns>new ordering status</returns>
    private static bool orderAgainQuerey(bool ordering)
    {
        Console.WriteLine("Would you like to order again? [y/n]");
        ConsoleKey response = Console.ReadKey(false).Key;
        if (response == ConsoleKey.N)
        {
            ordering = false;
        }
        else if (response != ConsoleKey.Y && response != ConsoleKey.N)
        {
            Console.WriteLine("Please provide a valid response.");
            orderAgainQuerey(ordering);
        }

        return ordering;
    }
    /// <summary>
    /// Determines which employee should pay for the order
    /// </summary>
    /// <param name="employees">List of participating employees.</param>
    /// <returns>Employee object of who should pay.</returns>
    private static string whoShouldPay(List<Employee> employees)
    {
        var lowestPastTotalPayments = employees.Min(item => item.PastTotalPayments);

        List<Employee> payCanidates = new List<Employee>();

        foreach (var item in employees)
        {
            if (item.PastTotalPayments == lowestPastTotalPayments)
            {
                payCanidates.Add(item);
            }
        }

        Random random = new Random();
        var paysToday = payCanidates[random.Next(payCanidates.Count)].Name;
        return paysToday;
    }
    /// <summary>
    /// Updates the stored value of the pastTotalPayments of the employee who paid.
    /// </summary>
    /// <param name="employees">List of participating employees</param>
    /// <param name="totalPayment">Total amount which is to be paid</param>
    /// <param name="paysToday">Name of employee who will pay today</param>
    private static void updateTotalPayments(List<Employee> employees, double totalPayment, string paysToday)
    {
        foreach (var item in employees)
        {
            if (item.Name.ToLower() == paysToday.Trim().ToLower())
            {
                item.PastTotalPayments = Math.Round((item.PastTotalPayments + totalPayment - item.LastPayment), 2);
            }
        }
    }
    /// <summary>
    /// Serialize objects and update JSON file
    /// </summary>
    /// <param name="path">path to the JSON file</param>
    /// <param name="list">List of employees</param>
    public static void updateJSON(string path, List<Employee> list)
    {
        File.Create(path).Close();
        using (StreamWriter sw = File.AppendText(path))
        {
            foreach (var i in list)
            {
                sw.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(i));
            }
        }
        return;
    }
    /// <summary>
    /// Reads user's key response and runs methods based on answer, loops if it fails.
    /// </summary>
    /// <param name="list">List of employees</param>
    /// <returns>Total amount to be paid by the payer</returns>
    public static double actOnResponse(List<Employee> list)
    {
        double totalPayment = 0.0;
        var quereyResponse = Console.ReadKey(true).Key;

        if (quereyResponse == ConsoleKey.Y)
        {
            totalPayment = didOrderUsuals(list, totalPayment);
        }
        else if (quereyResponse == ConsoleKey.N)
        {
            totalPayment = didNotOrderUsuals(list, totalPayment);
        }
        else
        {
            Console.WriteLine("Please provide a valid answer");
            actOnResponse(list);
        }
        return totalPayment;
    }
    /// <summary>
    /// reads stored usuals for employees, adds the prices and returns total
    /// </summary>
    /// <param name="list">List of employees</param>
    /// <param name="totalPayment">total payment made for the order</param>
    /// <returns>total amount paid</returns>
    private static double didOrderUsuals(List<Employee> list, double totalPayment)
    {
        foreach (var item in list)
        {
            totalPayment = totalPayment + item.Order;
            item.LastPayment = item.Order;
        }
        return totalPayment;
    }
    /// <summary>
    /// requests prices from user, adds the prices and returns total
    /// </summary>
    /// <param name="list">List of employees</param>
    /// <param name="totalPayment">total payment made for the order</param>
    /// <returns>total amount paid</returns>
    private static double didNotOrderUsuals(List<Employee> list, double totalPayment)
    {
        foreach (var item in list)
        {
            Console.WriteLine($"How much did {item.Name}'s order cost?");
            double price = 0.0;
            while (price == 0.0)
            {
                try
                {
                    price = double.Parse(Console.ReadLine());
                    item.LastPayment = price;
                }
                catch
                {
                    Console.WriteLine("Please input a valid price");
                }
            }
            totalPayment = totalPayment + price;
        }

        return totalPayment;
    }
    /// <summary>
    /// reads path. creates data if it doesn't exist or reads data from the file
    /// </summary>
    /// <param name="path">path of the json file</param>
    /// <returns>list of employee objects</returns>
    private static List<Employee> extractEmployees(string path)
    {
        List<Employee> list = new List<Employee>();

        if (!File.Exists(path) || new FileInfo(path).Length == 0) // create data if the file doesn't exist
        {
            list = generateEmployeeData(); // File was empty or didnt exist
        }
        else // extract data from CoffeCustomers.json
        {
            string[] values = File.ReadAllLines(path);
            foreach (var item in values)
            {
                var person = Newtonsoft.Json.JsonConvert.DeserializeObject<Employee>(item);
                list.Add(person);
            }
        }

        return list;
    }
    /// <summary>
    /// Generates new data for employees that order coffee
    /// </summary>
    /// <returns>list of employee objects</returns>
    private static List<Employee> generateEmployeeData()
    {
        List<Employee> list;
        Employee Bob = new Employee("Bob", 2.3);
        Employee Jeremy = new Employee("Jeremy", 4.5);
        Employee lorem = new Employee("Lorem", 6.4);
        Employee ipsum = new Employee("Ipsum", 5.6);
        Employee dolor = new Employee("Dolor", 7.8);
        Employee sit = new Employee("Sit", 1.3);
        Employee amet = new Employee("Amet", .5);
        list = [Bob, Jeremy, lorem, ipsum, dolor, sit, amet];
        return list;
    }
}

/// <summary>
/// Class containing data for employees.
/// </summary>
public class Employee
{
    public string Name { get; set; }
    public double Order { get; set; }
    public double PastTotalPayments { get; set; }
    public double LastPayment { get; set; }
    public Employee(string name, double order, double pastPayment = 0, double lastPayment = 0)
    {
        this.Name = name;
        this.Order = order;
        this.PastTotalPayments = pastPayment;
        this.LastPayment = lastPayment;
    }

    public override string ToString()
    {
        return $"{Name}, with the usual order of {Order}, has paied a total of: {PastTotalPayments.ToString("C2")}";
    }
}
