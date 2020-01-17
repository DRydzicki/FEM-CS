using System;
using System.Linq;

namespace FEM_Calculations
{
    class Program
    {
       
         static void Main()
        {

            var globalData = new Globaldata();
            int size = globalData.ME;
            Element[] el = new Element[size];
            for (var i = 0; i < el.Length; i++)
            {
                el[i] = new Element(i);
            }
            el = el.OrderBy(x => x.elementNumber).ToArray();

            for (var i = 0; i < el.Length; i++)
            {
                el[i].DisplayEl();
            }

            Console.WriteLine("\nOUTPUT:");
            globalData.GCalcualte(el);
            globalData.Display();
        }

        
       
    }
}
