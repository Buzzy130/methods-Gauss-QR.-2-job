using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using Com_Methods;

namespace ConsoleApp1
{

    internal class Program
    {
        static void Test_Gauss(int n)
        {
            Matrix A = new Matrix(n, n);
            A.Fill_values(2);

            Console.WriteLine("Cond A: " + A.Cond_Infinity());
            Vector Answer = new Vector(n);
            for (int i = 0; i < n; ++i)
            {
                Answer.Values[i] = 1.0;
            }
            Vector F = A * Answer;
            Vector RES = new Vector(A.M);
            GaussMethod Gauss = new GaussMethod();

            // Приводим к верхнетреугольному виду и решаем по Гауссу
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Gauss.Solve(A, F, RES);
            stopwatch.Stop();
            TimeSpan elapsed = stopwatch.Elapsed;

            double error = Vector.Eval_Error(Answer, RES);
            Console.WriteLine("Метода Гаусса с выбором ведущего элемента:");
            Console.Write("N: " + n);
            Console.WriteLine();
            Console.Write("Погрешность: " + error);
            Console.WriteLine();
            Console.Write("Время: " + elapsed.TotalSeconds + " с.");
            Console.WriteLine();
        }

        static void Test_Givens(int n)
        {
            Matrix A = new Matrix(n, n);
            Vector RES = new Vector(A.M);
            A.Fill_values(2);
            Vector Answer = new Vector(n);
            for (int i = 0; i < n; ++i)
            {
                Answer.Values[i] = 1.0;
            }
            Vector B = A * Answer;

            Stopwatch stopwatch = new Stopwatch();
            var Givens_Solver = new GivensMethod();
            stopwatch.Start();
            // Делаем QR разложение и все решаем
            Givens_Solver.Solve(A, B, RES);
            stopwatch.Stop();
            TimeSpan elapsed = stopwatch.Elapsed;

            double error = Vector.Eval_Error(Answer, RES);
            Console.WriteLine("Метода QR разложения Гивенса:");
            Console.Write("N: " + n);
            Console.WriteLine();
            Console.Write("Погрешность: " + error);
            Console.WriteLine();
            Console.Write("Время: " + elapsed.TotalSeconds + " с.");
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            Console.Write("Test of Cond Infinity on simple matrix");
            var A = new Matrix(2, 2);
            A.Values[0][0] = 1; A.Values[0][1] = 4;
            A.Values[1][0] = 4; A.Values[1][1] = 2;
            double val = A.Cond_Infinity();
            A.Print();
            Console.WriteLine("Cond Infinity: " + val);
            Console.WriteLine("--------------------------------\n");

            for (int i = 5; i <= 20; i += 5)
            {
                var B = new Matrix(i, i);
                B.Fill_values(1);

                Console.WriteLine("N = " + i + "\tCond Infinity: " + B.Cond_Infinity());
                Test_Gauss(i);
                Console.WriteLine();
                Test_Givens(i);
                Console.WriteLine();
            }
        }
    }
}
