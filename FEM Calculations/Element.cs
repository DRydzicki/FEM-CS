using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace FEM_Calculations
{

    
    class Element : Globaldata
    {
        private int node = 0;
        private int[] ID;
        public Matrix<double> H,Hbc;
        public double[] P = new double[2] { 0, 0 };

        
        public Element(int elNumber) 
        {
            int temp = elNumber * 3;
            ID = new int[] { elementBuilder[temp], elementBuilder[temp+1] };
            node = elementBuilder[temp + 2];
            double C = S * K / (L / ME);
            H = DenseMatrix.OfArray( new double[,] { { C, -C }, { -C, C } } );
            Hbc = DenseMatrix.OfArray( new double[,] { { 0, 0 }, { 0, 0 } } );
            //P = DenseMatrix.OfArray(new double[,] { 0, 0 });
            if (elNumber == 0 && node == 1)
            {
                P[0] = q * S;
            }
            else if (elNumber == 0 && node == 2)
            {
                P[0] = -1 * alpha * S * tinf;
                Hbc[0,0] = alpha * S;

            }
            else if (node == 1)
            {
                P[1] = q * S;
            }
            else if (node == 2)
            {
                P[1] = -1 * alpha * S * tinf;
                Hbc[1,1] = alpha * S;
            }
        }



        public void DisplayEl()
        {
            Console.WriteLine("Element " + ID[0] + ": ");
            Console.WriteLine("Nodes: " + ID[0] + ", " + ID[1]);

            Console.WriteLine("H: \n" + H[0,0] + " " + H[0,1]);
            Console.WriteLine( H[1, 0] + " " + H[1, 1]);

            Console.WriteLine("Hbc: \n" + Hbc[0, 0] + " " + Hbc[0, 1]);
            Console.WriteLine(Hbc[1, 0] + " " + Hbc[1, 1]);

            Console.WriteLine("P: \n" + P[0]);
            Console.WriteLine(P[1]);

            Console.WriteLine();
        }
    }
}
