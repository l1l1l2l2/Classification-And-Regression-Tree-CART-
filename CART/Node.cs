using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deedle;

namespace CART
{
    public class Node
    {
        public Node Left { get; set; }
        public Node Right { get; set; }
        public Frame<int, string> Data { get; set; }
        public int rowCount { get; set; }
        public string FeatureName { get; set; }
        public double? Threshold { get; set; }
        public double Gini { get; set; }
        public string PredictedClass { get; set; }
        public int Depth { get; set; }
        public bool IsLeaf { get => Left == null || Right == null; private set => IsLeaf = value; }
        public Dictionary<string, int> CountsDictionary { get; set; }
        public Node(Frame<int, string> data)
        {
            Data = data;
            rowCount = data.RowCount;
            CountsDictionary = Data.GetClassesCountDictionary();
            Gini = gini();
            PredictedClass = CountsDictionary.First(x => x.Value == CountsDictionary.Values.Max()).Key //***
;
        }
        private double gini()
        {
            int allCount = CountsDictionary.Sum(x => x.Value);
            return 1.0 - CountsDictionary.Sum(x => Math.Pow((double)x.Value / allCount, 2));
        }
    }
}
