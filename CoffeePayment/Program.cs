using System.Security.Cryptography.X509Certificates;

internal class Program
{
    

    private static void Main(string[] args)
    {
        
        Coffee Latte = new Coffee("latte",2.99);
        Console.WriteLine(Latte.Name);
        Console.WriteLine(Latte.price);

        Peepson Bob = new Peepson("Bob", Latte);

        Console.WriteLine(Bob.Name);
        Console.WriteLine(Bob.order.price);

        string path = Directory.GetCurrentDirectory();
        Console.WriteLine(path);
        Peepson[] list = new Peepson[7];
        list[1] = Bob;
                using (StreamWriter sw = File.AppendText(path))
                {
                    foreach (var line in list)
                    {
                        Peepson e = (Peepson)line; // unbox once
                        sw.WriteLine(e.Name);
                        sw.WriteLine(e.order);
                        sw.WriteLine(e.order.price);
                    }
                }

    }
}


class Peepson
{
    public readonly string Name;
    public readonly Coffee order;

    public Peepson(string name, Coffee order)
    {
        this.Name = name;
        this.order = order;
    }   
}
class Coffee
{
    public readonly string Name;
    public readonly double price;

    public Coffee(string name, double price)
    {
        this.Name = name;
        this.price = price;
    }
}