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
        private double _K, _S, _L, _alpha, _tinf, _q;
        private Matrix<double> _Hg, _Pg, _Tg, _temp;
        private int _MN,_ME;
        private List<int[]> _elementBuilder=new List<int[]>();
        private int lastNodeCheck = 0, firstNodeCheck = 0;

        public double K { get => _K; set => _K = value; }
        public double S { get => _S; set => _S = value; }
        public double L { get => _L; set => _L = value; }
        public double Alpha { get => _alpha; set => _alpha = value; }
        public double Tinf { get => _tinf; set => _tinf = value; }
        public double Q { get => _q; set => _q = value; }
        public Matrix<double> Hg { get => _Hg; set => _Hg = value; }
        public Matrix<double> Pg { get => _Pg; set => _Pg = value; }
        public Matrix<double> Tg { get => _Tg; set => _Tg = value; }
        public Matrix<double> Temp { get => _temp; set => _temp = value; }
        public int MN { get => _MN; set => _MN = value; }
        public int ME { get => _ME; set => _ME = value; }
        public List<int[]> ElementBuilder { get => _elementBuilder; set => _elementBuilder = value; }

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
                            if (Convert.ToInt32(keyAndValue[1]) > Convert.ToInt32(dataDictionary["MN"]))
                                throw new ArgumentException("Invalid number of nodes.");
                            if (Convert.ToInt32(keyAndValue[0]) == Convert.ToInt32(dataDictionary["MN"])  || Convert.ToInt32(keyAndValue[1]) == Convert.ToInt32(dataDictionary["MN"]))
                                lastNodeCheck++;
                            if (Convert.ToInt32(keyAndValue[1]) == 1)
                                throw new ArgumentException("First node cannot be last node of element.");
                            if (Convert.ToInt32(keyAndValue[0]) == Convert.ToInt32(keyAndValue[1]))
                                throw new ArgumentException("Element does not exist.");
                            int id1 = Convert.ToInt32(keyAndValue[0]);
                            int id2 = Convert.ToInt32(keyAndValue[1]);
                            int bc = Convert.ToInt32(keyAndValue[2]);
                            int[] temp = { id1, id2, bc };
                            ElementBuilder.Add(temp);
                            
                       }
                    }
                    if (lastNodeCheck != 1) 
                        throw new ArgumentException("Multiple elements in last node.");

                    K = Convert.ToDouble(dataDictionary["K"]);
                    S = Convert.ToDouble(dataDictionary["S"]);
                    L = Convert.ToDouble(dataDictionary["L"]);
                    Alpha = Convert.ToDouble(dataDictionary["alpha"]);
                    Tinf = Convert.ToDouble(dataDictionary["tinf"]);
                    Q = Convert.ToDouble(dataDictionary["q"]);
                    MN = Convert.ToInt32(dataDictionary["MN"]);
                    ME = MN - 1;        
                }
                

            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ArgumentException e1)
            {
                Console.WriteLine(e1.Message);
            }
        }
        
        public void GCalcualte(Element[] elements)
        {
            try
            {
                Hg = DenseMatrix.Create(MN, MN, 0);
                Pg = DenseMatrix.Create(MN, MN, 0);
                Tg = DenseMatrix.Create(MN, MN, 0);


                for (int i = 0; i < MN - 1; i++)
                {
                    Hg[i, i] += elements[i].H[0, 0] + elements[i].Hbc[0, 0];
                    Hg[i, i + 1] += elements[i].H[0, 1] + elements[i].Hbc[0, 1];
                    Hg[i + 1, i] += elements[i].H[1, 0] + elements[i].Hbc[1, 0];
                    Hg[i + 1, i + 1] += elements[i].H[1, 1] + elements[i].Hbc[1, 1];
                    Pg[i, 0] += elements[i].P[0];
                    Pg[i + 1, 0] += elements[i].P[1];
                }

                Temp = Hg.Inverse();
                Tg = Temp.ConjugateTransposeThisAndMultiply(Pg);
                Tg = Tg.Multiply(-1);
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ArgumentOutOfRangeException e1)
            {
                Console.WriteLine(e1.Message);
            }
        }
        public void Display()
        {
            try
            {
                Console.WriteLine("Global data:");
                Console.WriteLine("K: " + K);
                Console.WriteLine("S: " + S);
                Console.WriteLine("Q: " + Q);
                Console.WriteLine("Alpha: " + Alpha);
                Console.WriteLine("Ambient temp: " + Tinf);
                Console.WriteLine("Number of nodes: " + MN);
                Console.WriteLine("Number of elements: " + ME);
                Console.WriteLine("\n");
                Console.WriteLine("Matrix H: ");
                for (int i = 0; i < MN; i++)
                {
                    for (int j = 0; j < MN; j++)
                    {
                        Console.Write(Hg[i, j] + " ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("Matrix P:");
                for (int i = 0; i < MN; i++)
                {
                    Console.WriteLine(Pg[i, 0]);
                }
                Console.WriteLine("TEMPERATURES: ");
                for (int i = 0; i < MN; i++)
                {
                    Console.WriteLine(Tg[i, 0]);
                }
            }
            catch(NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

}
