using System;
using System.IO;
using Deedle;

namespace ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var root = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            var Data = Frame.ReadCsv(Path.Combine(root, "iris.csv"));
            var columb = Data.
            Console.WriteLine("-- Raw Data --");
            Data.Print();
            Console.WriteLine(Data.ToString());
        }
    }
}
