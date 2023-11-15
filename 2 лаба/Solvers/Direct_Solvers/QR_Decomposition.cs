using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com_Methods
{
    public interface IQr_Decomposition
    {
        double EPS { get; set; }
    }
    internal class Qr_Decomposition
    {
        public double EPS { get; set; }
        public Qr_Decomposition()
        {
            EPS = 0.000000000000000001;
        }

        public Qr_Decomposition(double eps)
        {
            EPS = eps;
        }
        public void Givens_QR(Matrix A, Matrix Q, Matrix R)
        {
            R.Copy(A);
            Q.Fill_ones();
            int m = A.M, n = A.N;
            for (int i = 0; i < n - 1; ++i)
            {
                for (int j = i + 1; j < m; ++j)
                {
                    if (Math.Abs(R.Values[j][i]) > EPS)
                    {
                        double denominator = Math.Sqrt(Math.Pow(R.Values[i][i], 2) + Math.Pow(R.Values[j][i], 2));
                        double cosine = R.Values[i][i] / denominator;
                        double sine = R.Values[j][i] / denominator;
                        for (int k = i; k < n; ++k)
                        {
                            double elem1 = cosine * R.Values[i][k] + sine * R.Values[j][k];
                            double elem2 = cosine * R.Values[j][k] - sine * R.Values[i][k];
                            R.Values[i][k] = elem1;
                            R.Values[j][k] = elem2;
                        }
                        for (int k = 0; k < m; ++k)
                        {
                            double elem1 = cosine * Q.Values[k][i] + sine * Q.Values[k][j];
                            double elem2 = cosine * Q.Values[k][j] - sine * Q.Values[k][i];
                            Q.Values[k][i] = elem1;
                            Q.Values[k][j] = elem2;
                        }
                    }
                }
            }
        }
    }
}
