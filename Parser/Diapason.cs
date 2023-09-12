using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    internal class Diapason
    {
        public readonly int a, b;

        public Diapason(int a, int b)
        {
            this.a = a; this.b = b;
        }

        public void print()
        {
            Debug.Print(a + " " + b);
        }
    }
}
