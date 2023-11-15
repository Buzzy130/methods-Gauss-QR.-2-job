using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Com_Methods
{
    public interface IVector
    {
        int N { get; set; }
        double EPS { get; set; }
    }
    internal class Vector : IVector
    {
        public int N { get; set; }
        public double EPS { get; set; }
        public double[] Values { get; set; }

        public Vector()
        {
            N = 100;
            Values = new double[N];
            for (int i = 0; i < N; ++i)
            {
                Values[i] = 0;
            }
            EPS = 0.00000000001;
        }

        public Vector(int size)
        {
            N = size;
            Values = new double[N];
            for (int i = 0; i < N; ++i)
            {
                Values[i] = 0;
            }
            EPS = 0.00000000001;
        }

        public static Vector operator *(Vector Vec, double val)
        {
            Vector Output = new Vector(Vec.N);
            for (int i = 0; i < Vec.N; ++i)
            {
                Output.Values[i] = Vec.Values[i] * val;
            }
            return Output;
        }

        public static double operator *(Vector Vec1, Vector Vec2)
        {
            if (Vec1.N != Vec2.N)
            {
                throw new ArgumentException("Vector's size must be the same");
            }
            double output = 0;
            for (int i = 0; i < Vec1.N; ++i)
            {
                output += Vec1.Values[i] * Vec2.Values[i];
            }
            return output;
        }

        public static Vector operator +(Vector Vec1, Vector Vec2)
        {
            if (Vec1.N != Vec2.N)
            {
                throw new ArgumentException("Vector's size must be the same");
            }
            Vector output = new Vector(Vec1.N);
            for (int i = 0; i < Vec1.N; ++i)
            {
                output.Values[i] = Vec1.Values[i] + Vec2.Values[i];
            }
            return output;
        }

        public static double Eval_Error(Vector Correct, Vector Solved)
        {
            double norm1 = Math.Sqrt(Correct * Correct);
            Vector Differ = Solved + (Correct * (-1.0));
            double norm2 = Math.Sqrt(Differ * Differ);
            if (norm1 < Correct.EPS)
            {
                throw new ArgumentException("Correct norm can't be equal zero");
            }
            return norm2 / norm1;
        }

        public Vector Copy()
        {
            Vector Output = new Vector(N);
            for (int i = 0; i < N; ++i)
            {
                Output.Values[i] = Values[i];
            }
            return Output;
        }
    }
}
