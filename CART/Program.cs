using System;
using Deedle;
using System.IO;
using System.Linq;

namespace CART
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = Frame.ReadCsv(
                Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "Iris.csv"),
                hasHeaders: false);

            var dataTest = Frame.ReadCsv(
                Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "Test.csv"),
                hasHeaders: false);
            
            Console.WriteLine("---Inpute Data---");
            data.Print();
            Console.WriteLine("---Test Data---");
            dataTest.Print();

            var dtc = new DecisionTreeClassifier();
            dtc.GrowTree(data, 10);

            var clData = dtc.PredictDataSet(dataTest).ToOrdinalSeries();
            Console.WriteLine("---Classified Data---");
            clData.Print();

        }

    }
}
