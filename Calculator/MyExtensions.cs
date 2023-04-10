using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public static class MyExtensions
    {
        public static bool IsBinaryOperator(this char c) => c == '+' || c == '-' || c == '*' || c == '/' || c == '^';
    }
}
