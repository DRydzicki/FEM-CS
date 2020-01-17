using System;
using System.Linq;

namespace FEM_Calculations
{
    class Program
    {

        static void Main()
        {
            //var watch = new System.Diagnostics.Stopwatch();
            //watch.Start();
            var globalData = new Globaldata();
            Calculations(globalData);
            Console.WriteLine("\nOUTPUT:");
            globalData.Display();

            //watch.Stop();
            //Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
        }
        public static void Calculations(Globaldata globalData)
        {
            try
            {
                var size = globalData.ME;
                var el = new Element[size];
                for (var i = 0; i < el.Length; i++)
                {
                    el[i] = new Element(globalData, i);
                }
                el = el.OrderBy(x => x.elementNumber).ToArray();

                for (var i = 0; i < el.Length; i++)
                {
                    el[i].Display();
                }
                globalData.GCalcualte(el);
            }
            catch(NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }
            catch(OverflowException e1)
            {
                Console.WriteLine(e1.Message);
            }
        }
    }
    
}
