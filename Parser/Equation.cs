using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class Equation
    {
        private Dictionary<string, double> dict = new Dictionary<string, double>();

        public Dictionary<string,double> Dict => dict;

        private double b;

        public double B => b;

        public Equation(Equation equation)
        { 
            List<string> keys = equation.Dict.Keys.ToList();

           foreach (string item in keys)
           {
                dict[item] = equation.Dict[item];
           }
           b = equation.b;

        }

        public Equation(string[] variableList)
        {
            foreach (string item in variableList)
            {
                dict[item] = 0;
            }

            b = 0;
        }
        public Equation(Dictionary<string, double> dictionary, double b)
        {
            foreach (var item in dictionary)
            {
                dict[item.Key] = item.Value;
            }

            this.b = b;
        }

        public void Plus(double value, string type)
        {
            if (type == "num")
            {
                b += value;
            }
            else
            {
                dict[type] += value;
            }
            

        }
        public void Plus(double value)
        {
            b += value;
        }

        public void print()
        {
            foreach (var VARIABLE in dict)
            {
                Debug.Print(VARIABLE.Key + " " + VARIABLE.Value);
            }
            Debug.Print(b.ToString());
            Debug.Print("---------------");
            //  Debug.Print(x + " " + y + " " + z + " " + b);
        }

        public int GetVarCount()
        {
            return dict.Count;
        }

        public double GetValueForVar(string item)
        {
            return dict[item];
        }
        public double GetValueForVar()
        {
            return b;
        }

        public bool IsLine()
        {
            foreach (var item in dict)
            {
                if (item.Value != 0)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsTry(List<char> chars, double[] roots)
        {
            double sum = 0;
            int i = 0;
            foreach (var item in Dict)
            {
                sum += item.Value*roots[i];
                i++;
            }

            return Math.Round(sum + B, 5) == 0;
        }

        public static bool CompareTo(Equation a, Equation b)
        {

            List<string> keys = a.Dict.Keys.ToList();
            if (Math.Abs(a.B - b.B) > 0.00001)
            {
                return false;
            }

            foreach (string item in keys)
            {
                if (Math.Abs(a.Dict[item] - b.Dict[item]) > 0.00001)
                {
                    return false;
                }
            }
            return true;
        }

    }
}