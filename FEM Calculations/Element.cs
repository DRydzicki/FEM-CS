using System;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace FEM_Calculations
{

    
    class Element
    {
        private int node = 0;
        private int[] ID;
        public Matrix<double> H,Hbc;
        public double[] P = new double[2] { 0, 0 };
        public int elementNumber;

        
        public Element(Globaldata gData,int elNumber) 
        {
            int[] temp = gData.elementBuilder[elNumber];
            elementNumber = temp[0];
            ID = new int[] { temp[0], temp[1] };         
            node = temp[2];
            double C = gData.S * gData.K / (gData.L / gData.ME);
            H = DenseMatrix.OfArray( new double[,] { { C, -C }, { -C, C } } );
            Hbc = DenseMatrix.OfArray( new double[,] { { 0, 0 }, { 0, 0 } } );
            if (elNumber == 0 && node == 1)
            {
                P[0] = gData.q * gData.S;
            }
            else if (elNumber == 0 && node == 2)
            {
                P[0] = -1 * gData.alpha * gData.S * gData.tinf;
                Hbc[0,0] = gData.alpha * gData.S;

            }
            else if (node == 1)
            {
                P[1] = gData.q * gData.S;
            }
            else if (node == 2)
            {
                P[1] = -1 * gData.alpha * gData.S * gData.tinf;
                Hbc[1,1] = gData.alpha * gData.S;
            }
        }



        public void Display()
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
