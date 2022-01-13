using System;
using System.IO;
using Deedle;

namespace ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var Data = Frame.ReadCsv(
                Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "iris.csv"), 
                hasHeaders:false);         
            Data.Print();
        }
    }
}
