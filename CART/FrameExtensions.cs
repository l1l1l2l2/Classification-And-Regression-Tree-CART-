using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deedle;

namespace CART
{
    public static class FrameExtensions
    {
        /// <summary>
        /// Повторений класса в фрейме
        /// </summary>
        public static Dictionary<string, int> GetClassesCountDictionary(this Frame<int,string> data)
        {

            Dictionary<string, int> counts = new Dictionary<string, int>();
            var classColumn = data.Columns.GetAt(data.Columns.KeyCount - 1);

            foreach(string value in classColumn.Values)
            {
                
                if (!counts.ContainsKey(value))
                    counts.Add(value, 1);
                else
                    counts[value]++;
            }   
            return counts;
        }
    }
}
