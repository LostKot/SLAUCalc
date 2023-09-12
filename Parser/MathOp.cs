using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    internal class MathOp
    {
        public static readonly char[] listOperation = { '*', '/', '^', '$', '-', '+' };
        public static readonly string[] funcOperation = { "sin", "cos", "tg", "ctg", "ln", "abs", "log" };

        public static double multy(double a, double b)
        {
            return a * b;
        }

        public static Equation multy(Equation a, Equation b, ref string error)
        {
            if (a.IsLine())
            {
                Dictionary<string,double> tempDictionary = new Dictionary<string,double>();
                foreach (var item in b.Dict)
                {
                    tempDictionary[item.Key] = Math.Round(item.Value*a.B,5);
                }
                return new Equation(tempDictionary,Math.Round(a.B*b.B,5));
            }

            if (b.IsLine())
            {
                Dictionary<string, double> tempDictionary = new Dictionary<string, double>();
                foreach (var item in a.Dict)
                {
                    tempDictionary[item.Key] = Math.Round(item.Value * b.B,5);
                }
                return new Equation(tempDictionary, Math.Round(a.B * b.B,5));
                // return new Equation(a.X * b.B, a.Y * b.B, a.Z * b.B, a.B * b.B);
            }

            error = "Уравнение трансцендентное";
            return new Equation(a.Dict,1);
        }

        public static double del(double a, double b, ref string error)
        {
            if (b == 0)
            {
                error = "Деление на ноль";
            }

            return a / b;
        }

        public static Equation del(Equation a, Equation b, ref string error)
        {
            if (!b.IsLine())
            {
                error = "Уравнение трансцендентное";
                return new Equation(a.Dict,1);
            }

            if (b.B == 0)
            {
                error = "Деление на ноль";
            }

            Dictionary<string,double> tempDictionary = new Dictionary<string,double>();

            foreach (var item in a.Dict)
            {
                tempDictionary[item.Key] = Math.Round(item.Value/b.B,5);
            }

            return new Equation(tempDictionary, Math.Round(a.B / b.B, 5));
        }

        public static double pow(double a, double b)
        {
            return Math.Round(Math.Pow(a, b), 5);
        }

        public static Equation pow(Equation a, Equation b, ref string error)
        {
            if (!a.IsLine() || !b.IsLine())
            {
                error = "Уравнение трансцендентное";
            }

            Dictionary<string,double> tempDictionary = new Dictionary<string,double>();

            foreach (var item in a.Dict)
            {
                tempDictionary[item.Key] = 0;
            }

            return new Equation(tempDictionary, Math.Round(Math.Pow(a.B, b.B), 5));
        }

        public static Equation minus(Equation a, Equation b)
        {
            Dictionary<string,double> dict = new Dictionary<string, double>();
            foreach (var item in a.Dict)
            {
                dict[item.Key] = item.Value - b.Dict[item.Key];
            }
            return new Equation(dict, a.B - b.B);
        }

        public static Equation plus(Equation a, Equation b)
        {
            Dictionary<string, double> dict = new Dictionary<string, double>();
            foreach (var item in a.Dict)
            {
                dict[item.Key] = item.Value + b.Dict[item.Key];
            }
            return new Equation(dict, a.B + b.B);
        }

        public static double getNumber(string x, ref string error)
        {
            if (x == "pi")
            {
                return Math.Round(Math.PI, 5);
            }

            if (x == "e")
            {
                return Math.Round(Math.E, 5);
            }

            try
            {
                return Math.Round(Convert.ToDouble(x), 5);
            }
            catch (FormatException)
            {
                Debug.Print(x.ToString());
                error = "Ошибка ввода или уравнение трансцендентное";

                return 1;
            }
        }

        public static double findOperation(double a, double b, string znak, ref string error)
        {
            if (znak == "/")
            {
                return Math.Round(del(a, b, ref error), 5);
            }
            else
            {
                return Math.Round(multy(a, b), 5);
            }
        }

        public static void ConvertToEquationLists(string workText, List<Scob> equationsList, string[] variableList ,List<Diapason> diapasonlist, ref string error, out List<Equation> equationsListOut, out string temptext) //Переводим сеобку в формат множества уравнений
        {
            StringBuilder sb = new StringBuilder();
            List<Equation> equations = new List<Equation>();
            int scobCount = 0;
            int skipiteration = -1;
            string tempText = ""; //Тут будут храниться операторы

            for (int i = 0; i < workText.Length; i++)
            {
                if (Array.IndexOf(listOperation, workText[i]) != -1 && i != 0)
                {
                    tempText += workText[i];
                    if (sb.ToString() != "")
                    {
                        equations.Add(toEquation(sb.ToString(), variableList, ref error));
                        sb.Clear();
                    }
                }

                else if (workText[i] == '(')
                {
                    equations.Add(equationsList[scobCount].equation);
                    i = diapasonlist[scobCount].b;
                    scobCount++;
                    sb.Clear();
                }
                else
                {
                    sb.Append(workText[i]);
                }
            }

            if (sb.ToString() != "")
            {
                equations.Add(toEquation(sb.ToString(), variableList, ref error));
            }

            temptext = tempText;
            equationsListOut = equations;
        }

        private static Equation toEquation(string text, string[] variableList, ref string error) //Перевод элемента в уравнение!!!!!!!
        {
            StringBuilder sb = new StringBuilder();
            string type = "";

            if (Parser.serchVar(text,variableList) != -1)
            {
                type = variableList[Parser.serchVar(text, variableList)];
                text = text.Replace(type, "");
            }
            sb.Append(text);
            

            if (sb.ToString() == "")
            {
                sb.Append("1");
            }

            Dictionary<string, double> tempDictionary = new Dictionary<string, double>();

            foreach (string item in variableList)
            {
                tempDictionary[item] = 0;
            }

            if (type == "")
            {
                return new Equation(tempDictionary, getNumber(sb.ToString(), ref error));
            }


            tempDictionary[type] = getNumber(sb.ToString(), ref error);

            return new Equation(tempDictionary, 0);
        }

        public static double findFunction(string function, Equation x, ref string error)
        {
            if (!x.IsLine())
            {
                error = "Уравнение трансцендентное";
                return 1;
            }

            if (function == "sin")
            {
                if (Math.Abs(Math.Sin(x.B)) < 0.00001)
                {
                    return 0;
                }

                return Math.Sin(x.B);
            }

            if (function == "cos")
            {
                if (Math.Abs(Math.Cos(x.B)) < 0.00001)
                {
                    return 0;
                }

                return Math.Cos(x.B);
            }

            if (function == "tg")
            {
                if (Math.Abs(Math.Cos(x.B)) < 0.00001)
                {
                    error = "Тангенс не существует";
                }
                if (Math.Abs(Math.Tan(x.B)) < 0.00001)
                {
                    return 0;
                }

                return Math.Tan(x.B);
            }

            if (function == "ctg")
            {
                if (Math.Abs(Math.Sin(x.B)) < 0.00001)
                {
                    error = "Катангенс не существует";
                }
                if (Math.Abs(1 / Math.Tan(x.B)) < 0.00001)
                {
                    return 0;
                }

                return 1 / Math.Tan(x.B);
            }

            if (function == "abs")
            {
                return Math.Abs(x.B);
            }

            if (function == "ln")
            {
                if (x.B <= 0)
                {
                    error = "Аргумент логарифма отрицателен";
                }

                if (Math.Abs(Math.Log(x.B)) < 0.00001)
                {
                    return 0;
                }

                return Math.Log(x.B);
            }

            if (function == "log")
            {
                if (x.B <= 0)
                {
                    error = "Аргумент логарифма отрицателен";
                }

                if (Math.Abs(Math.Log10(x.B)) < 0.00001)
                {
                    return 0;
                }

                return Math.Log10(x.B);
            }

            return 0;
        }
    }
}