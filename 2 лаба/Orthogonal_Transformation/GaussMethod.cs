using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com_Methods
{
    public interface IGaussMethod
    {
        double EPS { get; set; }
    }

    internal class GaussMethod : IGaussMethod
    {
        public double EPS { get; set; }

        public GaussMethod()
        {
            EPS = 0.000000000000000000000001;
        }

        public GaussMethod(double eps)
        {
            EPS = eps;
        }

        public void Solve(Matrix A, Vector F, Vector RES)
        {
            if (A.N != A.M)
            {
                throw new ArgumentException("A argument must be the square matrix");
            }
            if (A.N == 0)
            {
                throw new ArgumentException("A argument size mustn't be less then 1");
            }
            if (A.N != F.N)
            {
                throw new ArgumentException("A matrix size and F vector size must be the same");
            }

            Matrix A_copy = new Matrix(A.M, A.N);
            A_copy.Copy(A);
            Vector F_copy = F.Copy();
            for (int i = 0; i < A_copy.N; ++i)
            {
                int max_index = i;
                for (int j = i + 1; j < A_copy.N; ++j)
                {
                    if (Math.Abs(A_copy.Values[j][i]) > Math.Abs(A_copy.Values[max_index][i]))
                    {
                        max_index = j;
                    }
                }
                if (max_index != i)
                {
                    Vector Row1 = A_copy.Row(i), Row2 = A_copy.Row(max_index);
                    double elem1 = F_copy.Values[i], elem2 = F_copy.Values[max_index];
                    A_copy.Set_row(i, Row2);
                    A_copy.Set_row(max_index, Row1);
                    F_copy.Values[i] = elem2;
                    F_copy.Values[max_index] = elem1;
                }
                if (Math.Abs(A_copy.Values[i][i]) < EPS)
                {
                    throw new Exception("Matrix A is singular");
                }

                for (int j = i + 1; j < A_copy.N; ++j)
                {
                    double coeff = -A_copy.Values[j][i] / A_copy.Values[i][i];
                    A_copy.Set_row(j, A_copy.Row(j) + A_copy.Row(i) * coeff);
                    F_copy.Values[j] = F_copy.Values[j] + F_copy.Values[i] * coeff;
                }
                Solve_By_Triangular_Matrix(A_copy, F_copy, RES);
            }
        }
        public static void Solve_By_Triangular_Matrix(Matrix A, Vector F, Vector RES)
        {
            for (int i = A.N - 1; i >= 0; --i)
            {
                RES.Values[i] = F.Values[i];
                for (int j = i + 1; j < A.N; ++j)
                {
                    RES.Values[i] -= RES.Values[j] * A.Values[i][j];
                }
                RES.Values[i] /= A.Values[i][i];
            }
        }
    }
}
