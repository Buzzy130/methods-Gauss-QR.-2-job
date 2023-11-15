using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com_Methods
{
    public interface IGivensMethod
    {
        double EPS { get; set; }
    }
    internal class GivensMethod : IGivensMethod
    {
        public double EPS { get; set; }

        public GivensMethod()
        {
            EPS = 0.00000000000000000001;
        }

        public GivensMethod(double eps)
        {
            EPS = eps;
        }

        public void Solve(Matrix A, Vector B, Vector Res)
        {
            Qr_Decomposition qr = new Qr_Decomposition();
            Matrix Q = new Matrix(A.N, A.N);
            Matrix R = new Matrix(A.N, A.N);
            qr.Givens_QR(A, Q, R);
            Vector Y = Q.Transpose() * B;
            Console.WriteLine();
            GaussMethod.Solve_By_Triangular_Matrix(R, Y, Res);
        }
    }
}
