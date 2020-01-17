using System;

namespace FEM_Calculations
{
    class Program
    {
       
         static void Main()
        {

            var globalData = new Globaldata();
            int size = globalData.ME;
            var el = new Element[size];
            for (var i = 0; i < el.Length; i++)
            {
                el[i] = new Element(i);
            }

            /*for (var i = 0; i < el.Length; i++)
            {
                el[i].DisplayEl();
            }*/

            Console.WriteLine("\nOUTPUT:");
            globalData.GCalcualte(el);
            globalData.Display();
        }

        
       
    }
}
