using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deedle;


namespace CART
{

    public class DecisionTreeClassifier
    {
        private List<Series<int, string>> _predictionListResult { get; set; } = new List<Series<int, string>>();
        public Node _root { get; private set; }
        int MaxDepth { get; set; }
        public void GrowTree(Frame<int, string> data, int maxDepth)
        {
            MaxDepth = maxDepth;
            //root init
            _root = new Node(data);
            _root.Depth = 0;
            bestSplit(_root);
        }



        private void bestSplit(Node node)
        {
            //Находим пороговые значения
            var ThresholdesDict = new Dictionary<string, List<double>>();
            foreach(var name in node.Data.ColumnKeys)
            {
                if(name != node.Data.ColumnKeys.Last())
                {
                    var tempUniq = new List<double>();
                    foreach (var i in node.Data.RowKeys)
                    {
                        var value = (double)(decimal)node.Data[name, i];
                        if (!tempUniq.Contains(value))
                            tempUniq.Add(value);
                    }
                    ThresholdesDict.Add(name, tempUniq);
                }
            }

            //Перебираем пороговые значения
            //обработать исключения без if **
            Node tempLeftNode, tempRightNode, bestLeftNode, bestRightNode;
            bestLeftNode = null;
            bestRightNode = null;
            double deltaH = -1000000;
            double? bestThresholde = null;
            string bestFeatureKey = null;

            foreach (var key in ThresholdesDict.Keys)
            {
                foreach(var val in ThresholdesDict[key])
                {
                    double tempDeltaH;
                    var diffs = node.Data.Clone().Rows.Where(kvp => kvp.Value.GetAs<double>(key) >= val);
                    if (diffs.ValueCount == 0) //**
                        continue;
                    tempLeftNode = new Node(Frame.FromRows(diffs));
                    diffs = node.Data.Clone().Rows.Where(kvp => kvp.Value.GetAs<double>(key) < val);//clone*
                    if (diffs.ValueCount == 0) //**
                        continue;
                    tempRightNode = new Node(Frame.FromRows(diffs));
                    tempDeltaH = weightedGiniDelta(node, tempLeftNode, tempRightNode);
                    if (tempDeltaH > deltaH)
                    {
                        bestThresholde = val;
                        deltaH = tempDeltaH;
                        bestLeftNode = tempLeftNode;
                        bestRightNode = tempRightNode;
                        bestFeatureKey = key;
                    }
                }
            }
            if(bestLeftNode != null && bestRightNode != null)
            {
                node.FeatureName = bestFeatureKey;
                node.Left = bestLeftNode;
                node.Left.Depth = node.Depth + 1;
                node.Right = bestRightNode;
                node.Right.Depth = node.Depth + 1;
                node.Threshold = bestThresholde;
                if (node.Left.Depth < MaxDepth)
                {
                    if (node.Left.rowCount > 1)
                        bestSplit(node.Left);
                    if (node.Right.rowCount > 1)
                        bestSplit(node.Right);
                }
            }
        }
        public List<Series<int, string>> PredictDataSet(Frame<int, string> data, Node node = null)
        {
            if(data != null)
            {
                if (node == null)
                    node = _root;
                Frame<int, string> leftFrame = null;
                Frame<int, string> rightFrame = null;

                if (node.Threshold != null)
                {
                    var diffs = data.Clone().Rows.Where(kvp => kvp.Value.GetAs<double>(node.FeatureName) >= node.Threshold);
                    if (diffs.ValueCount != 0)
                        leftFrame = Frame.FromRows(diffs);
                    diffs = data.Rows.Where(kvp => kvp.Value.GetAs<double>(node.FeatureName) < node.Threshold);
                    if (diffs.ValueCount != 0)
                        rightFrame = Frame.FromRows(diffs);
                }



                if (!node.IsLeaf)
                {
                    PredictDataSet(leftFrame, node.Left);
                    PredictDataSet(rightFrame, node.Right);
                }
                else
                {

                    _predictionListResult.Add(data.Rows.Select(kvp =>
                    kvp.Value.GetAs<string>(data.ColumnKeys.Last()) + node.PredictedClass));
                }
            }
            
            return _predictionListResult;

        }   
        //delta H   
        private double weightedGiniDelta(Node nodeParent, Node nodeLeft, Node nodeRight)
        {
            return nodeParent.Gini - (nodeLeft.rowCount / nodeRight.rowCount * nodeLeft.Gini + nodeRight.rowCount / nodeLeft.rowCount * nodeRight.Gini);
        }
    }
}
