using System;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace FEM_Calculations
{

    
    class Element
    {
        private int _node = 0;
        private int[] _ID;
        private Matrix<double> _H,_Hbc;
        private double[] _P = new double[2] { 0, 0 };
        public int elementNumber;

        public int Node { get => _node; set => _node = value; }
        public int[] ID { get => _ID; set => _ID = value; }
        public Matrix<double> H { get => _H; set => _H = value; }
        public Matrix<double> Hbc { get => _Hbc; set => _Hbc = value; }
        public double[] P { get => _P; set => _P = value; }

        public Element(Globaldata gData,int elNumber) 
        {
            try
            {
                int[] temp = gData.ElementBuilder[elNumber];
                elementNumber = temp[0];
                ID = new int[] { temp[0], temp[1] };
                Node = temp[2];
                double C = gData.S * gData.K / (gData.L / gData.ME);
                H = DenseMatrix.OfArray(new double[,] { { C, -C }, { -C, C } });
                Hbc = DenseMatrix.OfArray(new double[,] { { 0, 0 }, { 0, 0 } });
                if (elNumber == 0 && Node == 1)
                {
                    P[0] = gData.Q * gData.S;
                }
                else if (elNumber == 0 && Node == 2)
                {
                    P[0] = -1 * gData.Alpha * gData.S * gData.Tinf;
                    Hbc[0, 0] = gData.Alpha * gData.S;

                }
                else if (Node == 1)
                {
                    P[1] = gData.Q * gData.S;
                }
                else if (Node == 2)
                {
                    P[1] = -1 * gData.Alpha * gData.S * gData.Tinf;
                    Hbc[1, 1] = gData.Alpha * gData.S;
                }
            }
            catch (NullReferenceException e) 
            {
                Console.WriteLine(e.Message);
            }
            catch(ArgumentOutOfRangeException e1)
            {
                Console.WriteLine(e1.Message);
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
