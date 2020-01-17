using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace FEM_Calculations
{
    class Globaldata 
    {
        protected double K, S, L, alpha, tinf, q;
        protected double dL;
        private Matrix<double> Hg, Pg, Tg, temp;
        public int MN,ME;
        //protected int[,] elementBuilder=new int[ME,3];
        protected List<int[]> elementBuilder=new List<int[]>();

        public Globaldata()
        {
            
            try
            {
                string dataContent = String.Empty;

                using (FileStream fs = File.Open("Data.txt", FileMode.Open))
                using (StreamReader reader = new StreamReader(fs))
                {
                    dataContent = reader.ReadToEnd();
                }
                if (dataContent.Length > 0)
                {
                    string[] lines = dataContent.Split(new char[] { '\n' });
                    Dictionary<string, string> dataDictionary = new Dictionary<string, string>();
                    foreach (string line in lines)
                    {
                        string[] keyAndValue = line.Split(new char[] { ' ' });
                        dataDictionary.Add(keyAndValue[0].Trim(), keyAndValue[1].Trim());
                        
                       if (keyAndValue[0].ToCharArray().Any(Char.IsDigit))
                       {
                            int id1 = Convert.ToInt32(keyAndValue[0]);
                            int id2 = Convert.ToInt32(keyAndValue[1]);
                            int bc = Convert.ToInt32(keyAndValue[2]);
                            int[] temp = { id1, id2, bc };
                            elementBuilder.Add(temp);
                            
                       }
                    }
                   
                    K = Convert.ToDouble(dataDictionary["K"]);
                    S = Convert.ToDouble(dataDictionary["S"]);
                    L = Convert.ToDouble(dataDictionary["L"]);
                    alpha = Convert.ToDouble(dataDictionary["alpha"]);
                    tinf = Convert.ToDouble(dataDictionary["tinf"]);
                    q = Convert.ToDouble(dataDictionary["q"]);
                    MN = Convert.ToInt32(dataDictionary["MN"]);
                    ME = MN - 1;        
                }

            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        public void GCalcualte(Element[] elements)
        {
            Hg = DenseMatrix.Create(MN, MN, 0);
            Pg = DenseMatrix.Create(MN, MN, 0);
            Tg = DenseMatrix.Create(MN, MN, 0);


            for (int i = 0; i < MN - 1; i++)
            {
                Hg[i,i] += elements[i].H[0,0] + elements[i].Hbc[0,0];
                Hg[i,i + 1] += elements[i].H[0,1] + elements[i].Hbc[0,1];
                Hg[i + 1,i] += elements[i].H[1,0] + elements[i].Hbc[1,0];
                Hg[i + 1,i + 1] += elements[i].H[1,1] + elements[i].Hbc[1,1];
                Pg[i,0] += elements[i].P[0];
                Pg[i + 1,0] += elements[i].P[1];
            }

            temp = Hg.Inverse();
            Tg = temp.ConjugateTransposeThisAndMultiply(Pg);
            Tg = Tg.Multiply(-1);
        }
        public void Display()
        {
            Console.WriteLine("Global data:");
            Console.WriteLine("K: " + K);
            Console.WriteLine("S: " + S);
            Console.WriteLine("Q: " + q);
            Console.WriteLine("Alpha: " + alpha);
            Console.WriteLine("Ambient temp: " + tinf);
            Console.WriteLine("Number of nodes: " + MN);
            Console.WriteLine("Number of elements: " + ME);
            Console.WriteLine("\n");
            Console.WriteLine("Matrix H: ");
            for (int i = 0; i < MN; i++)
            {
                for (int j = 0; j < MN; j++)
                {
                    Console.Write(Hg[i,j]+" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("Matrix P:");
            for (int i = 0; i < MN; i++)
            {
                Console.WriteLine(Pg[i,0]);
            }
            Console.WriteLine("TEMPERATURES: ");
            for (int i = 0; i < MN; i++)
            {
                Console.WriteLine(Tg[i, 0]);
            }
        }
    }

}
