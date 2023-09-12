using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Parser
{
    internal class Parts
    {
        private string type;
        public readonly string text;
        private double value;
        public readonly bool plus;
        public string TYPE => type;

        public double VALUE => value;

        public Parts(string type, string text, bool plus, ref string error)
        {
            StringBuilder sb = new StringBuilder();
            this.type = type;

            if (type != "num")
            {
                Regex reg = new Regex(type);
                text = reg.Replace(text, "P", 1);
            }

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == 'P')
                {
                    if (i == 0 && text != "P")
                    {
                        sb.Append('1');
                        continue;
                    }

                    if (i != 0 && text != "P"&&(text[i - 1] == '-' || text[i - 1] == '+'))
                    {
                        sb.Append('1');
                        continue;
                    }

                }
                if (text[i] == '*' && text[i + 1] == 'P')
                {
                    i++;
                }
                else if (text[i] == 'p' || text[i] == 'e') //нормализация Полученной строки. Проверка на форму записи 1e
                {
                    if (i != 0 && (text[i - 1] != '*' && text[i - 1] != '^' && text[i - 1] != '/'))
                    {
                        sb.Append("$" + text[i]);
                    }
                    else
                    {
                        sb.Append(text[i]);
                    }
                }
                else if (text[i] != 'P')
                {
                    sb.Append(text[i]);
                }
            }

            if (text == "P")
            {
                sb.Clear();
                sb.Append('1');
            }

          //  Debug.Print(sb.ToString());
            this.text = sb.ToString();
            this.plus = plus;
            pars(ref error);
            if (!plus)
            {
                value *= -1;
            }
        }

        private void pars(ref string error)
        {
            StringBuilder sb = new StringBuilder();
            string workText = text;


            if (workText.IndexOf('^') != -1)
            {
                workText = pows(workText, ref error);
            }

            if (text.IndexOf('$') != -1) //если есть запись 2e или 2pi
            {
                workText = favorite(workText, ref error);
            }

            string temp = "", znak = "";
            double a = 0, b = 0;
           // Debug.Print("^^"+workText);
            foreach (char item in workText)
            {
            //    Debug.Print(item.ToString());
                if (item == '*' || item == '/')
                {
                    if (znak == "")
                    {
                        a = MathOp.getNumber(sb.ToString(), ref error);
                        sb.Clear();
                    }
                    else
                    {
                        b = MathOp.getNumber(sb.ToString(), ref error);
                        sb.Clear();
                        a = MathOp.findOperation(a, b, znak, ref error);
                    }

                    znak = item.ToString();
                    continue;
                }

                sb.Append(item);
                
                // Debug.Print(znak + " " + a);
            }

            if (znak == "")
            {
                value = MathOp.getNumber(workText, ref error);
            }
            else
            {
                b = MathOp.getNumber(sb.ToString(), ref error);
                value = MathOp.findOperation(a, b, znak, ref error);
            }
        }

        private string pows(string worktext, ref string error) //Вычисляем степени
        {
            StringBuilder sb = new StringBuilder();
            string outtext = "";
            bool openStepen = false;
            double a, b = 1;
            for (int i = worktext.Length - 1; i >= 0; i--)
            {
                if (worktext[i] == '^')
                {
                    if (!openStepen)
                    {
                        openStepen = true;
                        char[] temp = sb.ToString().ToCharArray();
                        Array.Reverse(temp);
                        b = MathOp.getNumber(new string(temp), ref error);
                        sb.Clear();
                    }
                    else
                    {
                        char[] temp = sb.ToString().ToCharArray();
                        Array.Reverse(temp);
                        sb.Clear();
                        a = MathOp.getNumber(new string(temp), ref error);
                        b = MathOp.pow(a, b);
                    }
                }
                else if ((worktext[i] == '*' || worktext[i] == '/' || worktext[i] == '$') && openStepen)
                {
                    openStepen = false;
                    char[] temp = sb.ToString().ToCharArray();
                    Array.Reverse(temp);
                    sb.Clear();
                    a = MathOp.getNumber(new string(temp), ref error);
                    b = MathOp.pow(a, b);
                    temp = b.ToString().ToCharArray();
                    Array.Reverse(temp);
                    outtext += new string(temp) + worktext[i];
                }
                else if ((worktext[i] == '*' || worktext[i] == '/' || worktext[i] == '$') && !openStepen)
                {
                    sb.Append(worktext[i]);
                    outtext += sb.ToString();
                    sb.Clear();
                }
                else if (i == 0 && openStepen)
                {
                    sb.Append(text[i]);
                    char[] temp = sb.ToString().ToCharArray();
                    Array.Reverse(temp);
                    sb.Clear();
                    a = MathOp.getNumber(new string(temp), ref error);
                    b = MathOp.pow(a, b);
                    temp = b.ToString().ToCharArray();
                    Array.Reverse(temp);
                    outtext += new string(temp);
                }
                else
                {
                    sb.Append(worktext[i]);
                }
            }

            outtext += sb.ToString();
            char[] temptext = outtext.ToCharArray();

            Array.Reverse(temptext);

            return new string(temptext);
        }

        private string favorite(string text, ref string error) //если есть $
        {
            StringBuilder sb = new StringBuilder();
            string outstring = "";
            string temp = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (temp != "")
                {
                    if (text[i] == '*' || text[i] == '/' || text[i] == '^')
                    {
                        double a = MathOp.getNumber(temp, ref error);
                        double b = MathOp.getNumber(sb.ToString(), ref error);
                        outstring += MathOp.multy(a, b);
                        sb.Clear();
                        temp = "";
                        outstring += text[i];
                    }
                    else if (i == text.Length - 1)
                    {
                        double a = MathOp.getNumber(temp, ref error);
                        sb.Append(text[i]);
                        double b = MathOp.getNumber(sb.ToString(), ref error);
                        outstring += MathOp.multy(a, b);
                        sb.Clear();
                        temp = "";
                    }
                    else
                    {
                        sb.Append(text[i]);
                    }

                    continue;
                }


                if (text[i] == '*' || text[i] == '/' || text[i] == '^')
                {
                    sb.Append(text[i]);
                    outstring += sb.ToString();
                    sb.Clear();
                }
                else if (text[i] == '$')
                {
                    temp = sb.ToString();
                    sb.Clear();
                }
                else
                {
                    sb.Append(text[i]);
                }
            }

            outstring += sb.ToString();
            //  Debug.Print(outstring);
            return outstring;
        }

        public void print()
        {
            Debug.Print(text + " " + type + " " + plus + " " + value);
        }

        public Equation ToEquation(string[] variableList)
        {
            Dictionary<string,double> tempDictionary = new Dictionary<string,double>();

            foreach (string item in variableList)
            {
                tempDictionary[item] = 0;
            }

            if (type == "num")
            {
                return new Equation(tempDictionary, value);
            }
            tempDictionary[type] = value;
            return new Equation(tempDictionary, 0);
        }
    }
}