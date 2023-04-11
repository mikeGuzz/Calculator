using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string operand = string.Empty;
        private char operation;
        private bool showRes, outReset, invalidRes;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Count()
        {
            if (invalidRes || !operation.IsBinaryOperator())
                return;
            double op2;
            double op1;
            if (showRes)
            {
                op2 = double.Parse(operand);
                op1 = double.Parse(out_textBlock.Text);
            }
            else
            {
                op2 = double.Parse(out_textBlock.Text);
                op1 = string.IsNullOrEmpty(operand) ? op2 : double.Parse(operand);
            }
            equation_textBlock.Text = $"{op1} {operation} {op2} = ";
            switch (operation)
            {
                case '+':
                    out_textBlock.Text = (op1 + op2).ToString();
                    break;
                case '-':
                    out_textBlock.Text = (op1 - op2).ToString();
                    break;
                case '*':
                    out_textBlock.Text = (op1 * op2).ToString();
                    break;
                case '/':
                    if(op2 == 0)
                    {
                        out_textBlock.Text = "Not a number";
                        invalidRes = true;
                        return;
                    }
                    out_textBlock.Text = (op1 / op2).ToString();
                    break;
                case '^':
                    out_textBlock.Text = Math.Pow(op1, op2).ToString();
                    break;
            }
            if(!showRes)
                operand = op2.ToString();
            showRes = true;
            outReset = true;
        }

        private void ResetOut()
        {
            if (showRes)
            {
                if (equation_textBlock.Text != string.Empty)
                    equation_textBlock.Text = string.Empty;
            }
            else
                operand = out_textBlock.Text;
            outReset = false;
        }

        private void AddNum(string num)
        {
            if (outReset || invalidRes)
            {
                ResetOut();
                if (invalidRes)
                    ClearAll();
                out_textBlock.Text = num;
            }
            else if (out_textBlock.Text != "0")
                out_textBlock.Text += num;
            else 
                out_textBlock.Text = num;
        }

        private void NumButtonClick(object sender, RoutedEventArgs e)
        {
            AddNum((string)((FrameworkElement)e.Source).Tag);
        }

        private void SetOperator(char c)
        {
            if (invalidRes)
                return;
            if (operation.IsBinaryOperator() && !string.IsNullOrEmpty(operand) && !showRes)
                Count();
            showRes = false;
            outReset = true;
            operation = c;
            equation_textBlock.Text = out_textBlock.Text + $" {operation} ";
        }

        private void OperatorButtonClick(object sender, RoutedEventArgs e)
        {
            SetOperator(((string)((FrameworkElement)e.Source).Tag)[0]);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                switch (e.Key)
                {
                    case Key.D8:
                        SetOperator('*');
                        return;
                    case Key.OemPlus:
                        SetOperator('+');
                        return;
                    case Key.D6:
                        SetOperator('^');
                        return;
                }
            }
            switch (e.Key)
            {
                case Key.Back:   
                    if (invalidRes || outReset)
                    {
                        Clear();
                        return;
                    }
                    if (out_textBlock.Text.Length == 1)
                        out_textBlock.Text = "0";
                    else if (out_textBlock.Text.Length > 0)
                        out_textBlock.Text = out_textBlock.Text.Remove(out_textBlock.Text.Length - 1);
                break;
                case Key.OemMinus:
                    SetOperator('-');
                    break;
                case Key.OemQuestion:
                    SetOperator('/');
                    break;
                case Key.Escape:
                    ClearAll();
                    break;
                case Key.Return: 
                case Key.OemPlus:
                    Count();
                    break;
                default:
                    var intKey = (int)e.Key;
                    if (intKey >= (int)Key.D0 && intKey <= (int)Key.D9)
                        AddNum((intKey - (int)Key.D0).ToString());
                    break;
            }
        }

        private void ClearAll()
        {
            operation = '\0';
            operand = string.Empty;
            equation_textBlock.Text = string.Empty;
            out_textBlock.Text = "0";
            invalidRes = false;
            outReset = false;
            showRes = false;
        }

        private void Clear()
        {
            if (outReset)
                ResetOut();
            if (invalidRes)
                ClearAll();
            else
                out_textBlock.Text = "0";
        }

        private void ac_button_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        private void c_button_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private void Invert()
        {
            if (invalidRes || out_textBlock.Text == "0")
                return;
            if (outReset)
                ResetOut();
            var n = -double.Parse(out_textBlock.Text);
            out_textBlock.Text = n == -0 ? "0" : n.ToString();
        }

        private void invert_button_Click(object sender, RoutedEventArgs e)
        {
            Invert();
        }

        private void dot_button_Click(object sender, RoutedEventArgs e)
        {
            if (outReset || invalidRes)
            {
                ResetOut();
                if (invalidRes)
                    ClearAll();
                out_textBlock.Text = "0.";
            }
            else if (!out_textBlock.Text.Contains('.'))
                out_textBlock.Text += '.';
        }

        private void equal_button_Click(object sender, RoutedEventArgs e)
        {
            Count();
        }
    }
}
