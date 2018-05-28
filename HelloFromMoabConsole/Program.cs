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
            Console.WriteLine("Hello Cam. Watch out for Cactus.");
            Console.WriteLine("Hello Carson. Remember to wait for us at intersections.");
            Console.WriteLine("Hello Kevin. ...Kevin? Where's Kevin?");

            Console.Write("Enter estimated miles per hour for Carson:");
            string input = Console.ReadLine();

            double mph;
            if (double.TryParse(input, out mph))
            {
                double duration = 12.0 / mph;
                Console.WriteLine($"It will take Carson approximately {duration:0.00} hours to complete SlickRock");
            }

            Console.WriteLine("\r\nPress any key to exit.");
            Console.ReadKey();
        }
    }
}
