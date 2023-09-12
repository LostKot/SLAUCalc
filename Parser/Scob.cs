using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    internal class Scob
    {
        public string text;
        public bool plus;

        public readonly Equation equation;

        public List<Scob> childrenScobList = new List<Scob>();

        public Scob(Equation equation)
        {
            this.equation = equation;
            plus = true;
        }

        public Scob(string text, bool plus, string[] variableList, ref string error)
        {
            this.text = text;
            this.plus = plus;
            setCheldrenList(variableList,ref error);
            if (childrenScobList.Count == 0)
            {
                equation = Parser.parsScobParts(text,variableList, ref error);
            }
            else
            {
                normalize(variableList);
                equation = parsEquation(ref error, variableList);
            }
        }

        private bool searches(char text) //Проверяем есть ли функция
        {
            foreach (string VARIABLE in MathOp.funcOperation)
            {
                if (VARIABLE[0] == text)
                {
                    return true;
                }
            }

            return false;
        }

        private void normalize(string[] variableList)
        {
            string tempText = "";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                
                if (text[i] == '(' && i != 0 && text[i - 1] != '*' && text[i - 1] != '/' && text[i - 1] != '^' &&
                    text[i - 1] != '+' && text[i - 1] != '-')
                {
                    sb.Append("$" + text[i]);
                    //tempText += sb + "$" + text[i];
                    //sb.Clear();
                    continue;
                }

                if (searches(text[i]))
                {
                    if (i != 0 && text[i - 1] == '-')
                    {
                         sb.Append("1$" + text[i]);
                        //tempText += sb + "1$" + text[i];
                        //sb.Clear();
                        continue;
                    }

                    if (i != 0 && text[i - 1] >= '0' && text[i - 1] <= '9')
                    {
                         sb.Append("$" + text[i]);
                        //tempText +=sb + "$" + text[i];
                        //sb.Clear();
                        continue;
                    }
                }

                if (text[i] == 'p' || text[i] == 'e' || IsVariable(variableList, text[i]) ) //нормализация Полученной строки. Проверка на форму записи 1e
                {
                    if (i != 0 && (text[i - 1] != '*' && text[i - 1] != '^' && text[i - 1] != '/' &&
                                   text[i - 1] != '(') && text[i - 1] != '+' && text[i - 1] != '-')
                    {
                        sb.Append("$" + text[i]);
                    }
                    else
                    {
                        sb.Append(text[i]);
                    }
                }
                else
                {
                    sb.Append(text[i]);
                }
            }

            //text = tempText;
            text = sb.ToString();
        }

        public bool IsVariable(string[] variableList, char charV)
        {
            foreach (string item in variableList)
            {
                if (item[0] == charV)
                {
                    return true;
                }
            }
            return false;
        }

        public void print()
        {
            Debug.Print(text + " " + plus);
            foreach (var VARIABLE in childrenScobList)
            {
                VARIABLE.print();
            }
        }

        private void setCheldrenList(string[] variableList, ref string error) //Устанавливаем есть ли потомки
        {
            int a = -1;
            List<Diapason> diapasonlist = new List<Diapason>();
            bool openscob = false;
            int scobs = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '(' && scobs == 0)
                {
                    a = i;
                    scobs++;
                    continue;
                }

                if (text[i] == '(' && scobs != 0)
                {
                    scobs++;
                    continue;
                }

                if (text[i] == ')')
                {
                    scobs--;
                    if (scobs == 0)
                    {
                        diapasonlist.Add(new Diapason(a, i));
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < diapasonlist.Count; i++)
            {
                for (int j = diapasonlist[i].a + 1; j < diapasonlist[i].b; j++)
                {
                    sb.Append(text[j]);
                }

                childrenScobList.Add(new Scob(sb.ToString(), true, variableList, ref error));
                sb.Clear();
            }
        }

        private List<Diapason> getDiapasonList(string text) //Получаем новый диапазон скобок
        {
            StringBuilder sb = new StringBuilder();
            int scobCount = 0;
            List<Diapason> diapasonlist = new List<Diapason>();
            int a = -1;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '(' && scobCount == 0)
                {
                    a = i;
                    scobCount++;
                }
                else if (text[i] == '(' && scobCount != 0)
                {
                    scobCount++;
                }
                else if (text[i] == ')')
                {
                    scobCount--;
                    if (scobCount == 0)
                    {
                        diapasonlist.Add(new Diapason(a, i));
                    }
                }
            }

            return diapasonlist;
        }

        private Equation parsEquation(ref string error, string[] variableList)
        {
            StringBuilder sb = new StringBuilder();
            string workText = text;
            List<Equation> equationsList;
            List<Scob> childrenScob = childrenScobList;
            // Вычисляем функции
            string search = getSearchText(workText);
            foreach (string item in MathOp.funcOperation)
            {
                if (search.IndexOf(item) != -1)
                {
                    FuncCalc(workText, getDiapasonList(workText),variableList, ref error, out workText, out childrenScob);
                    break;
                }
            }

            //Переведем содержимое скобки в массив уравнений
            MathOp.ConvertToEquationLists(workText, childrenScob, variableList, getDiapasonList(workText), ref error,
                out equationsList,
                out workText);

            if (workText.IndexOf('^') != -1) //Вычисляем степени
            {
                pows(workText, equationsList, variableList, ref error, out workText, out equationsList);
            }

            if (workText.IndexOf('$') != -1) //если есть запись 2e или 2pi
            {
                favorite(workText, equationsList, variableList, ref error, out workText, out equationsList);
            }

            if (workText.IndexOf('*') != -1 || text.IndexOf('/') != -1) //умножаем и делим
            {
                MultyOrDel(workText, equationsList, variableList, ref error, out workText, out equationsList);
            }

            Equation tempEquation;
            if (equationsList.Count != 0)
            {
                tempEquation = equationsList[0];
            }
            else
            {
                tempEquation = childrenScob[0].equation;
            }

            for (int i = 0; i < workText.Length; i++)
            {
                if (workText[i] == '-')
                {
                    tempEquation = MathOp.minus(tempEquation, equationsList[i + 1]);
                }

                if (workText[i] == '+')
                {
                    tempEquation = MathOp.plus(tempEquation, equationsList[i + 1]);
                }
            }

            return tempEquation;
        }

        private string getSearchText(string text) //Получение строки для поиска функции
        {
            int scobCount = 0;
            StringBuilder sb = new StringBuilder();
            foreach (char item in text)
            {
                if (item == '(')
                {
                    scobCount++;
                    continue;
                }

                if (item == ')')
                {
                    scobCount--;
                    continue;
                }

                if (scobCount == 0)
                {
                    sb.Append(item);
                }
            }

            return sb.ToString();
        }

        private void FuncCalc(string workText, List<Diapason> diapasonList, string[] variableList, ref string error, out string workTextOut,
            out List<Scob> childrenScobsOut)
        {
            StringBuilder sb = new StringBuilder();
            List<Scob> tempScobs = new List<Scob>();
            Dictionary<string,double> dictionary = new Dictionary<string, double>();
            foreach (string item in variableList)
            {
                dictionary[item] = 0;
            }
            string tempText = "";
            int scobCount = 0;
            int scobB = -1;

            for (int i = 0; i < workText.Length; i++)
            {
                if (i <= scobB)
                {
                    sb.Append(workText[i]);
                    continue;
                }

                if (workText[i] == '$')
                {
                    if (Array.IndexOf(MathOp.funcOperation, sb.ToString()) != -1)
                    {
                        tempText += "()";
                        tempScobs.Add(new Scob(new Equation(dictionary, MathOp.findFunction(sb.ToString(), childrenScobList[scobCount].equation, ref error))));
                        i = diapasonList[scobCount].b;
                        scobCount++;
                    }
                    else
                    {
                        tempText += sb + "$";
                    }

                    sb.Clear();
                }
                else if (Array.IndexOf(MathOp.listOperation, workText[i]) != -1)
                {
                    tempText += sb.ToString() + workText[i];
                    sb.Clear();
                }
                else
                {
                    if (workText[i] == '(')
                    {
                        scobB = diapasonList[scobCount].b;
                        tempScobs.Add(childrenScobList[scobCount]);
                        scobCount++;
                    }

                    sb.Append(workText[i]);
                }
            }

            tempText += sb.ToString();
            workTextOut = tempText;
            childrenScobsOut = tempScobs;
        }

        //Умножение и деление
        private void MultyOrDel(string workText, List<Equation> equationsList, string[] variableList, ref string error, out string workTextOut,
            out List<Equation> equationsListOut)
        {
            StringBuilder sb = new StringBuilder();
            Equation temp = new Equation(variableList);
            List<Equation> tempEquationsList = new List<Equation>();
            if (equationsList.Count != 0)
            {
                temp = equationsList[0];
            }

            for (int i = 0; i < workText.Length; i++)
            {
                if (workText[i] == '*')
                {
                    temp = MathOp.multy(temp, equationsList[i + 1], ref error);
                }
                else if (workText[i] == '/')
                {
                    temp = MathOp.del(temp, equationsList[i + 1], ref error);
                }
                else
                {
                    sb.Append(workText[i]);
                    tempEquationsList.Add(temp);
                    temp = equationsList[i + 1];
                }
            }

            tempEquationsList.Add(temp);
            workTextOut = sb.ToString();
            equationsListOut = tempEquationsList;
        }

        private void pows(string workText, List<Equation> equationsList, string[] variableList, ref string error, out string workTextOut,
            out List<Equation> equationsListOut)
        {
            StringBuilder sb = new StringBuilder();
            List<Equation> tempEquationsList = new List<Equation>();
            Equation b = new Equation(variableList);
            bool openStepen = false;
            for (int i = workText.Length - 1; i >= 0; i--)
            {
                if (workText[i] == '^') //Если нашли степень
                {
                    if (!openStepen) //если степени небыло то записываем временную переменную
                    {
                        openStepen = true;
                        b = equationsList[i + 1];
                    }
                    else //если была то выичсляем
                    {
                        b = MathOp.pow(equationsList[i + 1], b, ref error);
                    }
                }
                else //если не степень
                {
                    sb.Append(workText[i]); //добавляем знак
                    if (!openStepen) // если степени небыло то добавляем уравнение в скобку
                    {
                        tempEquationsList.Add(equationsList[i + 1]);
                    }
                    else // если была то обнуляем скобку и вычисляем степень и записываем в список
                    {
                        openStepen = false;
                        tempEquationsList.Add(MathOp.pow(equationsList[i + 1], b, ref error));
                    }
                }
            }

            if (openStepen)
            {
                tempEquationsList.Add(MathOp.pow(equationsList[0], b, ref error));
            }
            else
            {
                tempEquationsList.Add(equationsList[0]);
            }

            char[] temptext = sb.ToString().ToCharArray();
            Array.Reverse(temptext);

            workTextOut = new string(temptext);
            tempEquationsList.Reverse();
            equationsListOut = tempEquationsList;
        }

        private void favorite(string workText, List<Equation> equationsList, string[] variableList, ref string error, out string workTextOut,
            out List<Equation> equationsListOut)
        {
            StringBuilder sb = new StringBuilder();
            List<Equation> tempEquationsList = new List<Equation>();
            Equation temp = new Equation(variableList);
            if (equationsList.Count != 0)
            {
                temp = equationsList[0];
            }

            for (int i = 0; i < workText.Length; i++)
            {
                if (workText[i] == '$')
                {
                    temp = MathOp.multy(temp, equationsList[i + 1], ref error);
                    if (i == workText.Length - 1)
                    {
                        tempEquationsList.Add(temp);
                    }
                }
                else
                {
                    sb.Append(workText[i]);
                    tempEquationsList.Add(temp);
                    temp = equationsList[i + 1];
                    if (i == workText.Length - 1)
                    {
                        tempEquationsList.Add(temp);
                    }
                }
            }

            workTextOut = sb.ToString();
            equationsListOut = tempEquationsList;
        }
    }
}