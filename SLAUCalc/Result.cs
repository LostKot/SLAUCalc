using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLAUCalc
{
    internal class Result
    {
        public readonly double[] roots;
        public readonly string errorText = "";
        public readonly string[] charList;

        public Result(string text)
        {
            errorText = text;
        }

        public Result(double[] roots, string[] charList)
        {
            this.roots = roots;
            this.charList = charList;
        }
    }
}