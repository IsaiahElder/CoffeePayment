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
        string path = Path.GetFullPath("..\\CoffeeCustomers.json");
        List<Peepson> list = extractEmployees(path);

        Console.WriteLine(" - - - - - - - Coffee bean - - - - - - - - ");
        Console.WriteLine("Did everyone order their usual this time? [y/n]");
        var qResponse1 = Console.ReadKey(true).Key;
        Console.WriteLine(qResponse1);


        double totalPayment = 0.0;

        if (qResponse1 == ConsoleKey.Y)
        {
            
            foreach (var item in list)
            {
                totalPayment = totalPayment + item.Order;
                item.LastPayment = item.Order;
            }
            Console.WriteLine(totalPayment);
        }
        else if (qResponse1 == ConsoleKey.N)
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
        }
        else
        {
            Console.WriteLine("Please provide a valid answer");
        }


        //Console.WriteLine("who paid this time?");
        //string payer = Console.ReadLine();
        var lowestPastTotalPayments = list.Min(item => item.PastTotalPayments);
        List<Peepson> payCanidates = new List<Peepson>();

        foreach (var item in list)
        {
            if(item.PastTotalPayments == lowestPastTotalPayments)
            {
                payCanidates.Add(item);
            }
        }
        //var paysToday = list.First(item => item.PastTotalPayments == lowestPastTotalPayments);

        Random random = new Random();
        var paysToday = payCanidates[random.Next(payCanidates.Count)].Name;


        Console.WriteLine($"{paysToday} should pay today");


        foreach (var item in list)
        {
            if (item.Name == paysToday.Trim().ToLower())
            {
                
                item.PastTotalPayments = item.PastTotalPayments + totalPayment - item.LastPayment;
                Console.WriteLine(totalPayment);
                Console.WriteLine(item.PastTotalPayments);
            }
        }


        



        File.Create(path).Close();
        using (StreamWriter sw = File.AppendText(path))
        {

            foreach (var i in list)
            {
                // Console.WriteLine(i);
                sw.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(i));
            }
        }
    }


    /// <summary>
    /// reads path. creates data if it doesn't exist or reads data from the file
    /// </summary>
    /// <param name="path">path of the json file</param>
    /// <param name="Latte"></param>
    /// <returns></returns>
    private static List<Peepson> extractEmployees(string path)
    {
        List<Peepson> list = new List<Peepson>();

        if (!File.Exists(path) || new FileInfo(path).Length == 0) // create data if the file doesn't exist
        {
            Peepson Bob = new Peepson("Bob", 2.3);
            Peepson Jeremy = new Peepson("Jeremy", 4.5);
            Peepson lorem = new Peepson("Lorem", 6.4);
            Peepson ipsum = new Peepson("Ipsum", 5.6);
            Peepson dolor = new Peepson("Dolor", 7.8);
            Peepson sit = new Peepson("Sit", 1.3);
            Peepson amet = new Peepson("Amet", .5);
            list = [Bob, Jeremy, lorem, ipsum, dolor, sit, amet];
            Console.WriteLine("file was empty or didn't exist");
        }
        else // extract data from CoffeCustomers.json
        {
            string[] values = File.ReadAllLines(path);
            foreach (var item in values)
            {
                var person = Newtonsoft.Json.JsonConvert.DeserializeObject<Peepson>(item);
                list.Add(person);
            }
        }

        return list;
    }
}


public class Peepson
{
    public string Name { get; set; }
    public double Order { get; set; }
    public double PastTotalPayments {  get; set; }
    public double LastPayment { get; set; }
    public Peepson(string name, double order, double pastPayment = 0, double lastPayment = 0)
    {
        this.Name = name;
        this.Order = order;
        this.PastTotalPayments = pastPayment;
        this.LastPayment = lastPayment;
    }

    public override string ToString()
    {
        return $"{Name}, with the order of {Order}, has paied a total of: {PastTotalPayments.ToString("C2")}"; 
    }
}
