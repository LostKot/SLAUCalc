using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLAUCalc
{
    internal static class Validator
    {
        public static readonly char[] listOperation = { '*', '/', '^', '-', '+' };
        public static readonly string[] funcOperation = { "sin", "cos", "ctg","tg", "ln", "abs", "log" };

        public static bool ValidVar(string[] variableList)
        {
           

            for (int i = 0; i < variableList.Length; i++)
            {
                if (variableList[i] == "")
                {
                    return false;
                }
                for (int j = 0; j < variableList.Length; j++)
                {
                    if (variableList[j].IndexOf(variableList[i]) != -1 && i != j)
                    {
                        return false;
                    }
                }
            }

            foreach (var item in funcOperation)
            {
                for (int i = 0; i < variableList.Length; i++)
                {
                    if (item.IndexOf(variableList[i]) != -1)
                    {
                        return false;
                    }
                }
            }

               return true;
        }

        private static bool IsNormalChar(string text, string[] variableList) //Проверяем на разрешенные символы
        {
            text = text.Replace("P", "S");

            foreach (string item in variableList)
            {
                text = text.Replace(item, "P");
            }

            text = text.Replace("e", "P");
            text = text.Replace("pi", "P");
            text = text.Replace("(", "P");
            text = text.Replace(")", "P");
            text = text.Replace("=", "P");
            text = text.Replace(",", "P");

            foreach (string item in funcOperation)
            {
                text = text.Replace(item, "P");
            }

            foreach (char item in text)
            {
                if (!(item >= '0' && item <= '9'))
                {
                    if (Array.IndexOf(listOperation, item) == -1 && item != 'P')
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static bool IsLineFunction(string text, string[] variableList)
        {
            foreach (string item in variableList)
            {
                text = text.Replace(item, "P");
            }

            bool isDel = false;

            foreach (char item in text)
            {
                if (item == '/')
                {
                    isDel = true;
                    continue;
                }

                if (isDel && item == 'P')
                {
                    return false;
                }

                if ((Array.IndexOf(listOperation, item) != -1 || item == '=') && item != '/')
                {
                    isDel = false;
                }
            }

            return true;
        }

        private static bool IsNotX1(string text, string[] variableList)
        {
            //text = text.Replace("x", "P");
            //text = text.Replace("y", "P");
            //text = text.Replace("z", "P");
            foreach (string item in variableList)
            {
                text = text.Replace(item, "P");
            }
            foreach (char item in listOperation)
            {
                text = text.Replace(item, 'O');
            }

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == 'P' && i != text.Length - 1 && text[i + 1] != 'O' && text[i + 1] != '=' &&
                    text[i + 1] != ')')
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsNormalPatern(string text, string[] variableList)
        {
            string[] stopPaterStrings = { "OO", "FF", "(O)", "CC", "O,", ",O" };
            if (Array.IndexOf(listOperation, text[0]) != -1 && text[0] != '-' || text[0] == ',')
            {
                return false;
            }

            if (Array.IndexOf(listOperation, text[^1]) != -1 || text[^1] == ',')
            {
                return false;
            }

            foreach (string item in variableList)
            {
                text = text.Replace(item, "P");
            }

            if (text.IndexOf("P^") != -1)
            {
                return false;
            }

            foreach (char item in listOperation)
            {
                text = text.Replace(item, 'O');
            }

            foreach (string item in funcOperation)
            {
                text = text.Replace(item, "F");
            }

            text = text.Replace("pi", "C");
            text = text.Replace("e", "C");

            //Проверяем
            foreach (string item in stopPaterStrings)
            {
                if (text.IndexOf(item) != -1)
                {
                    return false;
                }
            }

            return true;
        }


        public static string Normalize(string text) //Нормализация строки
        {
            text = text.Replace(" ", "");

            return text.Replace(".", ",");
        }

        public static string IsValid(string text, string[] variableList)
        {
            if (!IsNormalChar(text,variableList))
            {
                return "Недопустимые символы или конструкции";
            }

            if (!IsEquation(text,variableList))
            {
                return "Не найдено уравнение";
            }

            if (!IsNormalPatern(text,variableList))
            {
                return "Недопустимые конструкции";
            }

            if (!IsNotX1(text,variableList))
            {
                return "Неверная запись коэффициента перед переменной";
            }

            if (!IsLineFunction(text,variableList))
            {
                return "Функция нелинейная";
            }

            return "";
        }

        private static bool IsEquation(string text, string[] variableList) //Проверяем есть ли уравнение
        {
            //text = text.Replace("x", "P");
            //text = text.Replace("y", "P");
            //text = text.Replace("z", "P");
            foreach (string item in variableList)
            {
                text = text.Replace(item, "P");
            }
            if (Counter(text, '=') == 0 || Counter(text, '=') > 1)
            {
                return false;
            }

            if (Counter(text, 'P') == 0)
            {
                return false;
            }

            string[] temp = text.Split('=');

            for (int i = 0; i <= 1; i++)
            {
                if (temp[i].IndexOf('P') == -1 && temp[i].IndexOf('e') == -1 && temp[i].IndexOf("pi") == -1)
                {
                    bool isNormal = false;
                    foreach (char item in temp[i])
                    {
                        if (item >= '0' && item <= '9') //Если есть цифра то считаем, что уравнение есть. Так как была найдена переменная
                        {
                            isNormal = true;
                            break;
                        }
                    }

                    if (!isNormal)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static int Counter(string text, char patern) //подсчет всех вхождений
        {
            int counter = 0;
            foreach (char item in text)
            {
                if (item == patern)
                {
                    counter++;
                }
            }

            return counter;
        }
    }
}