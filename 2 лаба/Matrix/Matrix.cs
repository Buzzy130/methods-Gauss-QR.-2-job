using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Com_Methods
{
    public interface IMatrix
    {
        int M { get; set; }
        int N { get; set; }
    }
    internal class Matrix : IMatrix
    {
        public int M { get; set; }
        public int N { get; set; }
        public double[][] Values { get; set; }

        public Matrix()
        {
            M = 100;
            N = 100;
            Values = new double[M][];
            for (int i = 0; i < M; i++)
            {
                Values[i] = new double[N];
            }
        }

        public Matrix(int m, int n)
        {
            M = m;
            N = n;
            Values = new double[M][];
            for (int i = 0; i < M; i++)
            {
                Values[i] = new double[N];
            }
        }

        public Vector Row(int index)
        {
            if ((index < 0) || (index >= M))
            {
                throw new ArgumentException("Row index out of range");
            }
            Vector Output = new Vector(N);
            for (int i = 0; i < N; ++i)
            {
                Output.Values[i] = Values[index][i];
            }
            return Output;
        }

        public Vector Column(int index)
        {
            if ((index < 0) || (index >= N))
            {
                throw new ArgumentException("Column index out of range");
            }
            Vector Output = new Vector(M);
            for (int i = 0; i < M; ++i)
            {
                Output.Values[i] = Values[i][index];
            }
            return Output;
        }

        public void Set_row(int index, Vector Vec)
        {
            if ((index < 0) || (index >= M))
            {
                throw new ArgumentException("Row index out of range");
            }
            else if (Vec.N != N)
            {
                throw new ArithmeticException("Vector size must be equal matrix row size");
            }
            for (int i = 0; i < N; ++i)
            {
                Values[index][i] = Vec.Values[i];
            }
        }

        public void Set_columns(int index, Vector Vec)
        {
            if ((index < 0) || (index >= N))
            {
                throw new ArgumentException("Row index out of range");
            }
            else if (Vec.N != M)
            {
                throw new ArithmeticException("Vector size must be equal matrix column size");
            }
            for (int i = 0; i < M; ++i)
            {
                Values[i][index] = Vec.Values[i];
            }
        }

        public void Fill_values(int type)
        {
            for (int i = 0; i < M; ++i)
            {
                for (int j = 0; j < N; ++j)
                {
                    if (type == 1)
                    {
                        if (i != j)
                        {
                            Values[i][j] = 0.1 + i + 0.5 * j;
                        }
                        else if (i == j)
                        {
                            Values[i][j] = 100;
                        }
                    }
                    else if (type == 2)
                    {
                        Values[i][j] = 2 / (double)(i + j + 0.5);
                    }
                    else
                    {
                        Values[i][j] = 0;
                    }
                }
            }
        }

        public void Fill_ones()
        {
            for (int i = 0; i < M; ++i)
            {
                for (int j = 0; j < N; ++j)
                {
                    Values[i][j] = (i == j) ? 1 : 0;
                }
            }
        }

        public static Vector operator *(Matrix A, Vector Vec)
        {
            if (A.N != Vec.N)
            {
                throw new ArgumentException("Number of columns of matrix A must be equal of Vec size");
            }
            Vector Output = new Vector(A.M);
            for (int i = 0; i < A.M; ++i)
            {
                Output.Values[i] = 0;
            }
            for (int i = 0; i < A.M; ++i)
            {
                for (int j = 0; j < A.N; ++j)
                {
                    Output.Values[i] += A.Values[i][j] * Vec.Values[j];
                }
            }
            return Output;
        }

        public void Print()
        {
            for (int i = 0; i < M; ++i)
            {
                for (int j = 0; j < N; ++j)
                {
                    Console.Write(Values[i][j] + " ");
                }
                Console.WriteLine();
            }
        }

        public Matrix Transpose()
        {
            Matrix T = new Matrix(N, M);
            for (int i = 0; i < M; ++i)
            {
                for (int j = 0; j < N; ++j)
                {
                    T.Values[j][i] = Values[i][j];
                }
            }
            return T;
        }


        public void Copy(Matrix A)
        {
            if (N != A.N || M != A.M)
            {
                throw new ArgumentException("Matrix sizes must be equal");
            }
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    Values[i][j] = A.Values[i][j];
                }
            }
        }

        delegate void Thread_Solver(int num);

        public double Cond_Infinity()
        {
            if (M != N)
            {
                throw new Exception("M must be equal N");
            }

            var QR_Decomp = new Qr_Decomposition();
            Matrix R = new Matrix(M, M);
            Matrix Q = new Matrix(M, M);
            QR_Decomp.Givens_QR(this, Q, R);
            if (Math.Abs(R.Values[M - 1][M - 1]) < QR_Decomp.EPS)
            {
                throw new Exception("Matrix is singular");
            }

            int num_threads = Environment.ProcessorCount;
            var semaphores = new bool[num_threads];
            var norm_row1 = new double[num_threads];
            var norm_row2 = new double[num_threads];
            var Start_Solver = new Thread_Solver((num) =>
            {
                int begin = N / num_threads * num;
                int end = begin + N / num_threads;
                var Givens_Solver = new GivensMethod();
                if (num + 1 == num_threads)
                {
                    end += N % num_threads;
                }

                for (int i = begin; i < end; ++i)
                {
                    var A = new Vector(N);
                    var A_inv = new Vector(N);
                    double summ1 = 0, summ2 = 0;
                    A.Values[i] = 1.0;
                    Givens_Solver.Solve(this.Transpose(), A, A_inv);
                    for (int j = 0; j < N; ++j)
                    {
                        summ1 += Math.Abs(Values[i][j]);
                        summ2 += Math.Abs(A_inv.Values[j]);
                    }

                    if (norm_row1[num] < summ1)
                        norm_row1[num] = summ1;
                    if (norm_row2[num] < summ2)
                        norm_row2[num] = summ2;
                }

                semaphores[num] = true;
            });

            for (int i = 0; i < num_threads; ++i)
            {
                int num = num_threads - i - 1;
                ThreadPool.QueueUserWorkItem((Param) => Start_Solver(num));
            }

            Start_Solver(0);
            while (Array.IndexOf<bool>(semaphores, false) != -1) Thread.Sleep(100);

            for (int i = 1; i < num_threads; ++i)
            {
                if (norm_row1[0] < norm_row1[i])
                    norm_row1[0] = norm_row1[i];
                if (norm_row2[0] < norm_row2[i])
                    norm_row2[0] = norm_row2[i];
            }

            return norm_row1[0] * norm_row2[0];
        }

    }
}
