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
            int size = globalData.ME;
            Element[] el = new Element[size];
            for (var i = 0; i < el.Length; i++)
            {
                el[i] = new Element(globalData,i);
            }
            el = el.OrderBy(x => x.elementNumber).ToArray();

            for (var i = 0; i < el.Length; i++)
            {
                el[i].Display();
            }

            Console.WriteLine("\nOUTPUT:");
            globalData.GCalcualte(el);
            globalData.Display();

            //watch.Stop();
            //Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");

        }

    }
}
