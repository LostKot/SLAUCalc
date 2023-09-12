using System.Diagnostics;
using Parser;

namespace SLAUCalc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Equation> lists = new List<Equation>(); //Храним спарсеные уравнения
            List<string> stringsMas = new List<string>(); //Храним нормализованные строки
            string[] variableList = textBox1.Text.Split(',');
            if (inTextBox.Text == "" || textBox1.Text == "")
            {
                outTextBox.Text = "Поле ввода пустое";
            }
            else
            {
                string error = "";

                bool isValid = true;


                if (!Validator.ValidVar(variableList))
                {
                    isValid = false;
                    error = "Ошибка в объявлении переменных";
                }

                if (isValid)
                {
                    foreach (string item in inTextBox.Lines)
                    {
                        string temp = Validator.Normalize(item);

                        error = Validator.IsValid(temp, variableList);
                        if (error != "")
                        {
                            isValid = false;
                            break;
                        }

                        stringsMas.Add(temp);

                    }
                }

                if (variableList.Length < inTextBox.Lines.Length)
                {
                    error = "Кол-во уравнений больше кол-ва переменных";
                    isValid = false;
                }

                Result result = new Result(error);
                //isValid = true;
                if (isValid)
                {
                    error = "";
                    foreach (string item in stringsMas)
                    {
                        lists.Add(Parser.Parser.pars(item, variableList, ref error));
                        if (error != "")
                        {
                            break;
                        }

                    }

                    foreach (var VARIABLE in lists)
                    {
                        VARIABLE.print();
                    }
                    if (error != "")
                    {
                        result = new Result(error);
                    }
                    else
                    {
                        //   result = Kramer.Solve(lists);
                        result = Gaus.Solve(lists, variableList);
                    }
                }

                if (result.errorText != "")
                {
                    outTextBox.Text = result.errorText;
                }
                else
                {
                    //Вывод результата
                    string outText = "Результат:^";
                    for (int i = 0; i < result.charList.Length; i++)
                    {
                        outText += result.charList[i] + ": " + Math.Round(result.roots[i], 5) + "^";
                    }

                    outTextBox.Lines = outText.Split('^');
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            int value = Convert.ToInt32(numericUpDown1.Value);
            int raz = value.ToString().Length;
            string outText = "";

            for (int i = 1; i <= value; i++)
            {
                string temp = "x";
                if (i.ToString().Length != raz)
                {
                    for (int j = 0; j < raz - i.ToString().Length; j++)
                    {
                        temp += "0";
                    }

                    temp += i;
                }
                else
                {
                    temp += i;
                }
                outText += temp + ',';
            }
            Debug.Print(outText);
            string temp2 = "";

            for (int i = 0; i < outText.Length-1; i++)
            {
                temp2 += outText[i];
            }

            textBox1.Text = temp2;


        }
    }
}