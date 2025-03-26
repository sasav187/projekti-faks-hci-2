using System;   

namespace hci_projekat2.Models
{
    public class CalculatorModel
    {
        public double Calculate(double a, double b, string operation)
        {
            return operation switch
            {
                "+" => a + b,
                "-" => a - b,
                "×" => a * b,
                "÷" => b != 0 ? a / b : throw new DivideByZeroException(),
                _ => throw new InvalidOperationException("Nepoznata operacija!")
            };
        }

        public double ScientificCalculate(double value, string function)
        {
            return function switch
            {
                "sin" => Math.Sin(value),
                "cos" => Math.Cos(value),
                "tan" => Math.Tan(value),
                "log" => Math.Log10(value),
                "ln" => Math.Log(value),
                "√" => Math.Sqrt(value),
                "x²" => Math.Pow(value, 2),
                "x³" => Math.Pow(value, 3),
                "10ˣ" => Math.Pow(10, value),
                "π" => Math.PI,
                "e" => Math.E,
                "±" => value * -1.0,
                _ => throw new InvalidOperationException("Nepoznata operacija!")
            };
        }

        public int BitwiseCalculate(int a, int b, string operation)
        {
            return operation switch
            {
                "AND" => a & b,
                "OR" => a | b,
                "XOR" => a ^ b,
                "NOT" => ~a,
                "<<" => a << b,
                ">>" => a >> b,
                _ => throw new InvalidOperationException("Nepoznata operacija!")
            };
        }

        public string ConvertToBase(int value, int baseValue)
        {
            return baseValue switch
            {
                2 => Convert.ToString(value, 2),
                16 => Convert.ToString(value, 16).ToUpper(),
                _ => value.ToString()
            };
        }

    }
}