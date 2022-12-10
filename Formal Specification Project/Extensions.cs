
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Integration;
using System.Windows;


namespace Formal_Specification_Project {
    public static class Extensions {
        public static string ReplaceAt(this string value, int index, char newchar) {
            if (value.Length <= index)
                return value;
            else
                return string.Concat(value.Select((c, i) => i == index ? newchar : c));
        }
    }
    
}
