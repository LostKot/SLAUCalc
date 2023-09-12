using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser;

namespace SLAUCalc
{
    internal static class Gaus
    {
        public static Result Solve(List<Equation> listsEquation,string[] variabliList)
        {
            int varCount = variabliList.Length;
            int equCount = listsEquation.Count;

            if (varCount == equCount)
            {
                int r = findRang(GetMatrix(listsEquation, varCount, false));
                int e = findRang(GetMatrix(listsEquation, varCount, true));

                Debug.Print(r.ToString());
                Debug.Print(e.ToString());
                if (r == e)
                {
                    return Method(GetMatrix(listsEquation, varCount, true), varCount, variabliList);
                }
                return new Result("Система не имеет решений");

            }
            if (varCount > equCount)
            {
                return new Result("Система не имеет решений");
            }
            return new Result("Система не имеет решений");
        }

        public static Result Method(double[,] coefficients, int n, string[] variableList)
        {
            double[] solution = new double[n];

            for (int i = 0; i < n; i++)
            {
                if (coefficients[i, i] == 0)
                {
                    int swapRow = -1;
                    for (int j = i + 1; j < n; j++)
                    {
                        if (coefficients[j, i] != 0)
                        {
                            swapRow = j;
                            break;
                        }
                    }

                    if (swapRow == -1)
                    {
                        return new Result("Система имеет бесконечное множество решений");
                    }

                    for (int k = 0; k <= n; k++)
                    {
                        (coefficients[i, k], coefficients[swapRow, k]) = (coefficients[swapRow, k], coefficients[i, k]);
                    }
                }

                double pivot = coefficients[i, i];

                for (int j = i + 1; j < n; j++)
                {
                    double factor = coefficients[j, i] / pivot;
                    for (int k = i; k <= n; k++)
                    {
                        coefficients[j, k] -= factor * coefficients[i, k];
                    }
                }
            }

            for (int i = n - 1; i >= 0; i--)
            {
                double sum = 0;
                for (int j = i + 1; j < n; j++)
                {
                    sum += coefficients[i, j] * solution[j];
                }

                solution[i] = (coefficients[i, n] - sum) / coefficients[i, i];
            }

            return new Result(solution,variableList);
        }

        private static double[,] GetMatrix(List<Equation> listsEquation,int n, bool expand)
        {
            double[,] matrix;
            int col = n;
            if (expand)
            {
                col++;
                matrix = new double[n, n + 1];
            }
            else
            {
                matrix = new double[n, n];
            }
            

            for (int i = 0; i < n; i++)
            {
             
                List<double> values = new List<double>();
                foreach (var VARIABLE in listsEquation[i].Dict)
                {
                    values.Add(VARIABLE.Value);
                }

                if (expand)
                {
                    values.Add(-listsEquation[i].B);
                }
              

                for (int j = 0; j < col; j++)
                {
                    matrix[i,j] = values[j];
                }
            }
            return matrix;
        }

        public static int findRang(double[,] matrix)
        {

            int coll = matrix.GetLength(0);
            int row = matrix.GetLength(1);

            int rank = coll;

            for (int n = 0; n < coll; n++)
            {
                bool zeroString = true;

                for (int rowCount = n; rowCount < row; rowCount++)
                {
                    if (matrix[n, rowCount] != 0)
                    {
                        zeroString = false;

                        if (rowCount != n)
                        {
                            for (int i = 0; i < coll; i++)
                            {
                                (matrix[i, rowCount], matrix[i, n]) = (matrix[i, n], matrix[i, rowCount]);
                            }

                        }

                        for (int i = n + 1; i < coll; i++)
                        {
                            double multy = matrix[i, n] / matrix[n, n];

                            for (int j = n; j < row; j++)
                            {
                                matrix[i, j] -= multy * matrix[n, j];
                            }
                        }

                        break;
                    }
                }

                if (zeroString)
                {
                    rank--;
                }

            }

            return rank;
        }
    }
    
}
