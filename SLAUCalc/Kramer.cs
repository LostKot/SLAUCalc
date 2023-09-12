using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Parser;

namespace SLAUCalc
{
    internal static class Kramer
    {
        //public static Result Solve(List<Equation> equationList)
        //{
        //    List<char> charLsit = GetCountVar(equationList);
        //    int variableCount = charLsit.Count;
        //    int equationCount = equationList.Count;

        //    if (variableCount == equationCount)
        //    {
        //        return Method(equationList, charLsit);
        //    }

        //    if (variableCount > equationCount)
        //    {
        //        return new Result("Система не имеет решений");
        //    }

        //    if (variableCount == 1)
        //    {
        //        Result result = Method(new List<Equation>() { equationList[0] }, charLsit);
        //        if (result.errorText != "") //Если система не решилась
        //        {
        //            return result;
        //        }

        //        if (valid(equationList, result))
        //        {
        //            return result;
        //        }

        //        return new Result("Система не имеет решений");
        //    }

        //    List<int> idList = new List<int>(); //Тут будут храниться id взятых уравнений
        //    Dictionary<char, string> dictVariable = new Dictionary<char, string>(); //Словарь

        //    foreach (char item in new char[] { 'x', 'y', 'z' })
        //    {
        //        if (charLsit.IndexOf(item) != -1)
        //        {
        //            dictVariable.Add(item, "false");
        //        }
        //        else
        //        {
        //            dictVariable.Add(item, "NaN");
        //        }
        //    }

        //    for (int i = 0; i < equationCount; i++) //Набираем уравнения с одной переменной
        //    {
        //        if (idList.Count == variableCount)
        //        {
        //            break;
        //        }

        //        if (equationList[i].GetVarCount() == 1)
        //        {
        //            foreach (char item in new char[] { 'x', 'y', 'z' })
        //            {
        //                if (equationList[i].GetValueForVar(item) != 0 && dictVariable[item] == "false" &&
        //                    isNotReply(equationList, idList, i))
        //                {
        //                    idList.Add(i);
        //                    dictVariable[item] = "true";
        //                    break;
        //                }
        //            }
        //        }
        //    }


        //    for (int i = 0; i < equationCount; i++) //Набираем уравнения с 2-мя переменными
        //    {
        //        if (idList.Count == variableCount)
        //        {
        //            break;
        //        }

        //        if (equationList[i].GetVarCount() == 2)
        //        {
        //            foreach (char item in new char[] { 'x', 'y', 'z' })
        //            {
        //                if (equationList[i].GetValueForVar(item) != 0 && dictVariable[item] == "false" &&
        //                    idList.IndexOf(i) == -1 && isNotReply(equationList, idList, i))
        //                {
        //                    idList.Add(i);
        //                    dictVariable[item] = "true";
        //                }
        //            }
        //        }
        //    }

        //    if (idList.Count != equationCount)
        //    {
        //        for (int i = 0; i < equationCount; i++) //Добираем уравнения если не хватило
        //        {
        //            if (idList.Count == variableCount)
        //            {
        //                break;
        //            }

        //            foreach (char item in new char[] { 'x', 'y', 'z' })
        //            {
        //                if (equationList[i].GetVarCount() == variableCount && dictVariable[item] == "false" &&
        //                    idList.IndexOf(i) == -1 && isNotReply(equationList, idList, i))
        //                {
        //                    idList.Add(i);
        //                    dictVariable[item] = "true";
        //                }
        //            }
        //        }
        //    }

        //    List<Equation> temp = new List<Equation>();
        //    foreach (int item in idList)
        //    {
        //        temp.Add(equationList[item]);
        //    }

        //    Result result2 = Method(temp, charLsit);
        //    if (result2.errorText != "") //Если система не решилась
        //    {
        //        return result2;
        //    }

        //    if (valid(equationList, result2))
        //    {
        //        return result2;
        //    }

        //    return new Result("Система не имеет решений");
        //}

        //private static bool isNotReply(List<Equation> equationList, List<int> idList, int id)
        //{
        //    foreach (int item in idList)
        //    {
        //        if (Equation.CompareTo(equationList[item], equationList[id]))
        //        {
        //            return false;
        //        }
        //    }

        //    return true;
        //}

        //private static bool valid(List<Equation> equationList, Result result)
        //{
        //    for (int i = 0; i < equationList.Count; i++) //соотносим уравнения
        //    {
        //        if (!equationList[i].IsTry(result.charList, result.roots))
        //        {
        //            return false;
        //        }
        //    }

        //    return true;
        //}

        //private static Result Method(List<Equation> equationList, List<char> charLsit)
        //{
        //    int n = equationList.Count;
        //    if (n != charLsit.Count)
        //    {
        //        return new Result("Уравнение решений не имеет");
        //    }

        //    double[,] matrix = getMatrix(equationList, charLsit);
        //    if (n == 3)
        //    {
        //        double det = matrix[0, 0] * matrix[1, 1] * matrix[2, 2] + matrix[0, 1] * matrix[1, 2] * matrix[2, 0] +
        //                     matrix[0, 2] * matrix[1, 0] * matrix[2, 1];
        //        det += -matrix[0, 0] * matrix[1, 2] * matrix[2, 1] - matrix[0, 1] * matrix[1, 0] * matrix[2, 2] -
        //               matrix[0, 2] * matrix[1, 1] * matrix[2, 0];
        //        double x = (matrix[0, 3] * matrix[1, 1] * matrix[2, 2] + matrix[0, 1] * matrix[1, 2] * matrix[2, 3] +
        //                    matrix[0, 2] * matrix[1, 3] * matrix[2, 1] - matrix[0, 3] * matrix[1, 2] * matrix[2, 1] -
        //                    matrix[0, 1] * matrix[1, 3] * matrix[2, 2] -
        //                    matrix[0, 2] * matrix[1, 1] * matrix[2, 3]) / det;

        //        double y = (matrix[0, 0] * matrix[1, 3] * matrix[2, 2] + matrix[0, 3] * matrix[1, 2] * matrix[2, 0] +
        //                    matrix[0, 2] * matrix[1, 0] * matrix[2, 3] - matrix[0, 0] * matrix[1, 2] * matrix[2, 3] -
        //                    matrix[0, 3] * matrix[1, 0] * matrix[2, 2] -
        //                    matrix[0, 2] * matrix[1, 3] * matrix[2, 0]) / det;

        //        double z = (matrix[0, 0] * matrix[1, 1] * matrix[2, 3] + matrix[0, 1] * matrix[1, 3] * matrix[2, 0] +
        //                    matrix[0, 3] * matrix[1, 0] * matrix[2, 1] - matrix[0, 0] * matrix[1, 3] * matrix[2, 1] -
        //                    matrix[0, 1] * matrix[1, 0] * matrix[2, 3] -
        //                    matrix[0, 3] * matrix[1, 1] * matrix[2, 0]) / det;

        //        if (det != 0)
        //        {
        //            return new Result(new double[] { x, y, z }, charLsit);
        //        }

        //        if (x == 0 && y == 0 && z == 0)
        //        {
        //            return new Result("Система имеет бесконечное кол-во решений");
        //        }

        //        return new Result("Система не имеет решений");
        //    }
        //    else if (n == 2)
        //    {
        //        double det = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
        //        double x = (matrix[0, 2] * matrix[1, 1] - matrix[0, 1] * matrix[1, 2]) / det;
        //        double y = (matrix[0, 0] * matrix[1, 2] - matrix[0, 2] * matrix[1, 0]) / det;
        //        if (det != 0)
        //        {
        //            return new Result(new double[] { x, y }, charLsit);
        //        }

        //        if (x == 0 && y == 0)
        //        {
        //            return new Result("Система имеет бесконечное кол-во решений");
        //        }

        //        return new Result("Система не имеет решений");
        //    }
        //    else if (n == 1)
        //    {
        //        if (matrix[0, 0] != 0)
        //        {
        //            return new Result(new double[] { matrix[0, 1] / matrix[0, 0] }, charLsit);
        //        }

        //        if (matrix[0, 0] == matrix[0, 1])
        //        {
        //            return new Result("Уравнение имеет бесконечное кол-во решений");
        //        }

        //        return new Result("Уравнение решений не имеет");
        //    }
        //    else
        //    {
        //        return new Result("Уравнение решений не имеет");
        //    }
        //}

        //private static double[,] getMatrix(List<Equation> equationList, List<char> charLsit)
        //{
        //    //проверяем на наличие переменных
        //    bool x = charLsit.IndexOf('x') != -1;
        //    bool y = charLsit.IndexOf('y') != -1;
        //    bool z = charLsit.IndexOf('z') != -1;

        //    int n = equationList.Count;
        //    double[,] matrix = new double[n, n + 1];

        //    for (int i = 0; i < n; i++)
        //    {
        //        bool tempx = x, tempy = y, tempz = z;
        //        for (int j = 0; j < n; j++)
        //        {
        //            if (tempx)
        //            {
        //                matrix[i, j] = equationList[i].X;
        //                tempx = false;
        //                continue;
        //            }

        //            if (tempy)
        //            {
        //                matrix[i, j] = equationList[i].Y;
        //                tempy = false;
        //                continue;
        //            }

        //            if (tempz)
        //            {
        //                matrix[i, j] = equationList[i].Z;
        //                tempz = false;
        //            }
        //        }

        //        matrix[i, n] = -1 * equationList[i].B;
        //    }

        //    return matrix;
        //}


        //private static List<char>
        //    GetCountVar(List<Equation> equationList) //Получаем массив символов с имеющимися переменными
        //{
        //    //Проверяем какие переменные присутствуют
        //    List<char> charLsit = new List<char>();
        //    foreach (Equation item in equationList)
        //    {
        //        if (item.X != 0 && charLsit.IndexOf('x') == -1)
        //        {
        //            charLsit.Add('x');
        //        }

        //        if (item.Y != 0 && charLsit.IndexOf('y') == -1)
        //        {
        //            charLsit.Add('y');
        //        }

        //        if (item.Z != 0 && charLsit.IndexOf('z') == -1)
        //        {
        //            charLsit.Add('z');
        //        }

        //        if (charLsit.Count == 3)
        //        {
        //            break;
        //        }
        //    }

        //    return charLsit;
        //}
    }
}