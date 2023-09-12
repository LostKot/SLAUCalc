using System.IO;
using System.Text;

namespace Parser
{
    public static class Parser
    {
        public static Equation pars(string text, string[] variableList, ref string error)
        {
            string[] temptext = text.Split('=');
            List<Parts> listParts = new List<Parts>();
            List<Scob> listScob = new List<Scob>();
            List<Parts> listParts2 = new List<Parts>();
            List<Scob> listScob2 = new List<Scob>();
            Equation temEquation = new Equation(variableList);

            parseCicle(temptext[0], true,variableList, ref error, out listParts, out listScob);
            parseCicle(temptext[1], false,variableList ,ref error, out listParts2, out listScob2);

            listParts.AddRange(listParts2);
            listScob.AddRange(listScob2);

            if (listParts.Count != 0)
            {
                temEquation = listParts[0].ToEquation(variableList);
                for (int i = 1; i < listParts.Count; i++)
                {
                    temEquation = MathOp.plus(temEquation, listParts[i].ToEquation(variableList));
                }
            }

            if (listScob.Count != 0)
            {
                if (listParts.Count == 0)
                {
                    if (listScob[0].plus)
                    {
                        temEquation = listScob[0].equation;
                    }
                    else
                    {
                        Dictionary<string,double> tempDictionary = new Dictionary<string,double>();

                        foreach (string item in variableList)
                        {
                            tempDictionary[item] = 0;
                        }
                        temEquation = MathOp.multy(new Equation(tempDictionary, -1), listScob[0].equation, ref error);
                    }
                }
                else
                {
                    if (listScob[0].plus)
                    {
                        temEquation = MathOp.plus(temEquation, listScob[0].equation);
                    }
                    else
                    {
                        temEquation = MathOp.minus(temEquation, listScob[0].equation);
                    }
                }

                for (int i = 1; i < listScob.Count; i++)
                {
                    if (listScob[i].plus)
                    {
                        temEquation = MathOp.plus(temEquation, listScob[i].equation);
                    }
                    else
                    {
                        temEquation = MathOp.minus(temEquation, listScob[i].equation);
                    }
                }
            }

            return temEquation;
        }

        //Основной парсер
        private static void parseCicle(string text, bool isPlus, string[] variableList, ref string error, out List<Parts> listPartsOut,
            out List<Scob> listScobOut)
        {
            StringBuilder sb = new StringBuilder();
            string type = "";
            string textvalue = ""; //Значение которое будет передаваться в части
            bool plus = isPlus;
            List<Parts> listParts = new List<Parts>();
            List<Scob> listScob = new List<Scob>();
            int openScobs = 0;
            foreach (char item in text)
            {
                if (item == '(') //проверяем скобки и открываем переменную
                {
                    type = "scob";
                    openScobs++;
                    sb.Append(item);
                    continue;
                }

                if (item == ')')
                {
                    sb.Append(item);
                    openScobs--;
                    continue;
                }

                if ((item == '-' || item == '+') && sb.Length == 0) // Проверяем положительность выражения
                {
                    if (item == '-')
                    {
                        if (isPlus)
                        {
                            plus = false;
                        }
                        else
                        {
                            plus = true;
                        }
                    }

                    continue;
                }

                if (sb.Length != 0 && (item == '-' || item == '+') &&
                    openScobs == 0) // Добовляем слагаемое, в нее не входит скобка. Иначе жобавляем скобку
                {
                    if (type == "")
                    {
                        type = "num";
                    }

                    textvalue = sb.ToString();

                    if (type == "scob")
                    {
                        listScob.Add(new Scob(textvalue, plus,variableList, ref error));
                    }
                    else
                    {
                        listParts.Add(new Parts(type, textvalue, plus, ref error));
                    }

                    sb.Clear();
                    type = "";
                    textvalue = "";

                    if (item == '-')
                    {
                        if (isPlus)
                        {
                            plus = false;
                        }
                        else
                        {
                            plus = true;
                        }
                    }
                    else
                    {
                        if (isPlus)
                        {
                            plus = true;
                        }
                        else
                        {
                            plus = false;
                        }
                    }

                    continue;
                }

                if (serchVar(sb.ToString()+item,variableList) !=-1 && type != "scob") //устанавливает тип перемеенной если он уже не установлен
                {
                    type = variableList[serchVar(sb.ToString() + item, variableList)];
                    sb.Append(item);
                    continue;
                }

                sb.Append(item);
            }

            if (type == "")
            {
                type = "num";
            }

            textvalue = sb.ToString();

            if (type == "scob")
            {
                listScob.Add(new Scob(textvalue, plus,variableList, ref error));
            }
            else
            {
                listParts.Add(new Parts(type, textvalue, plus, ref error));
            }

            listPartsOut = listParts;
            listScobOut = listScob;
        }

        internal static int serchVar(string text, string[] variableList)
        {
            for (int i = 0; i < variableList.Length; i++)
            {
                if (text.IndexOf(variableList[i]) != -1)
                {
                    return i;
                }
            }
            return -1;
        }


        internal static Equation parsScobParts(string text, string[] variableList, ref string error) //Парсим значения внутри скобок
        {
            StringBuilder sb = new StringBuilder();
            List<Parts> listParts = new List<Parts>();
            string type = "";
            string textvalue;
            bool plus = true;
            foreach (char item in text)
            {
                if ((item == '-' || item == '+') && sb.Length == 0) // Проверяем положительность выражения
                {
                    if (item == '-')
                    {
                        plus = false;
                    }

                    continue;
                }

                if (sb.Length != 0 && (item == '-' || item == '+')) // Добовляем слагаемое
                {
                    if (type == "")
                    {
                        type = "num";
                    }

                    textvalue = sb.ToString();

                    listParts.Add(new Parts(type, textvalue, plus, ref error));

                    sb.Clear();
                    type = "";
                    textvalue = "";

                    if (item == '-')
                    {
                        plus = false;
                    }
                    else
                    {
                        plus = true;
                    }

                    continue;
                }

                if (serchVar(sb.ToString() + item, variableList) != -1) //устанавливает тип перемеенной
                {
                    type = variableList[serchVar(sb.ToString() + item, variableList)];
                    sb.Append(item);
                    continue;
                }

                sb.Append(item);
            }

            if (type == "")
            {
                type = "num";
            }

            textvalue = sb.ToString();
            listParts.Add(new Parts(type, textvalue, plus, ref error));
            sb.Clear();
            Equation equation = new Equation(variableList);

            foreach (Parts item in listParts)
            {
                equation.Plus(item.VALUE, item.TYPE);
            }

            return equation;
        }
    }
}