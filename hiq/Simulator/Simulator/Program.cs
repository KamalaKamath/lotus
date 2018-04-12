using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugarSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sugar Simulator Initializing...");

            if (Init())
            {
                Console.WriteLine("Press Enter to Start Simulator or Contine \r\nType\r\n quit - To Exit the Simulator\r\n index -To Display of food and exercise Index\r\n");
                RunSimulator();
            }

            else
            {
              Console.WriteLine("Sugar Simulator startup failure...Simulator cannot be run now :-(");
            }

            Console.WriteLine("____________________________________________________________________");

            Console.WriteLine("Bye");

            Console.ReadLine();
        }

        private static bool Init()
        {
            /*Failures would have been logged by dependent components */
            /*Logging will be done here also for the SugarSimulator exe that it failed to run */

            return SimpleSimulator.Init();
        }
       public static void GetInput(ref List<Data> foodList, ref List<Data> exerciseList, int counter)
        {
            Console.WriteLine("Input Data - Format: Time-Hr(0-23){Space}Food or Excercise Index");
            Console.WriteLine("Example: 8 66,9 8,11 120,19 51");
            Console.WriteLine();

            Console.WriteLine(String.Format("Run - {0}", counter));
            Console.WriteLine(String.Format("Enter Food Intake Data for Run {0}", counter));

            string[] food = Console.ReadLine().Split(',');
            if (food.Length > 0 && food[0] != "")
            {
                foodList = food.Select(s => s.Split(' ')).Select(i => new Data { Index = Int32.Parse(i[1]), Hour = Int32.Parse(i[0]) }).ToList();
            }
            Console.WriteLine();
            Console.WriteLine(String.Format("Enter Exercise Activity Data for Run {0}", counter));

            string[] exercise = Console.ReadLine().Split(',');

            if (exercise.Length > 0 && exercise[0] != "")
            {
                exerciseList = exercise.Select(s => s.Split(' ')).Select(i => new Data { Index = Int32.Parse(i[1]), Hour = Int32.Parse(i[0]) }).ToList();
            }
        }

        private static void RunSimulator()
        {
            var counter = 0;

            var input = Console.ReadLine();

            while (input != "quit")
            {
                if (input == "index")
                {
                    SimpleSimulator.PrintIndex();
                    Console.WriteLine();
                    Console.WriteLine("Press Enter to continue or type quit to exit");
                    input = Console.ReadLine();
                }
                else
                {
                    counter++;
                    List<Data> foodList = new List<Data>();
                    List<Data> exerciseList = new List<Data>();
                   
                    GetInput(ref foodList, ref exerciseList, counter);
                  
                    SimpleSimulator.Simulate(foodList, exerciseList, counter);
                    Console.WriteLine(String.Format("Simulation COMPLETED for input dataset {0}. Press Enter to continue or type quit to exit", counter));
                    input = Console.ReadLine();
                }
            }
        }
    }

    /*Simple class for a data input */
    public class Data
    {
        public string Type { get; set; }
        public int Index { get; set; }
        public int Hour { get; set; }
    }
}
