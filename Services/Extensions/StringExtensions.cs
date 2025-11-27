using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Extensions
{
    public static class StringExtensions
    {
        public static string FormatNumber(decimal price)
        {
            string formatted = price.ToString("#,##0.00");
            return formatted;
        }
        public static List<string> SplitDescription(string discription)
        {
            var ListDiscription = discription.Split(',').ToList();
            return ListDiscription;
        }
    }
}
