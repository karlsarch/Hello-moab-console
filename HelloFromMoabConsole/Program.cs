using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloFromMoabConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Moab!");
            Console.WriteLine("Hello Cam. Watch out for Cactus and Uphill Sections.");
            Console.WriteLine("Hello Carson. Remember to wait for us at intersections.");
            Console.WriteLine("Hello Kevin. ...Kevin? Where's Kevin?");

            Console.Write("Enter estimated miles per hour for Carson:");
            string input = Console.ReadLine();

            if (double.TryParse(input, out double mph))
            {
                double duration = 10.7 / mph; // Slickrock is actually only 10.7 miles long, not 12.  
                Console.WriteLine($"It will take Carson approximately {duration:0.00} hours to complete SlickRock");
            }

            Console.WriteLine("\r\nPress any key to exit.");
            Console.ReadKey();
        }
    }
}
