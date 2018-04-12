using System;
using System.Collections.Generic;
using SugarSimulator.DataProvider;

namespace SugarSimulator
{
    public class Database
    {
        public static Dictionary<int, Tuple<int, string>> FoodIndex = null;
        public static Dictionary<int, Tuple<int, string>> ExerciseIndex = null;
        public static bool Success = false;
        
        /* 
          A Sugar simulator requires a food & exercise index database.
          And should implement two interfaces Init() and PrintIndex()
          The database can consume the data from any provider, based on the configuation or whatever the provider component it is intansiated with
          dependency injection
          
         */
        public static void Init()
        {
            try
            {

                /* We could have many providers of data. We could decide which provider to go based on dependency injection*/
                /* The data provider exposes two interface methods.. LoadFoodIndex*/
                var dataProvider = new FileDataProvider();

                Console.WriteLine("...Loading the Food Index");
                dataProvider.LoadFoodIndex(ref FoodIndex);

                Console.WriteLine("...Loading the Exercise Index");
                dataProvider.LoadExerciseIndex(ref ExerciseIndex);

                Success = true;
                Console.WriteLine("...Database Load SUCCESS");
            }
            catch
            {
                //We implement logs and errors, why the food and index download was a failure. 
                //The calling Simulator main program will exit. So we wil read the logs and errors and troubleshoot.
                Console.WriteLine("...Database Load FAILED..");
            }

            Console.WriteLine("-----------------------------------------------------------------");
        }

        public static void PrintIndex()
        {
            if (Success)
            {
                Console.WriteLine("--------------------------------------------------------");
                Console.WriteLine("FOOD INDEX");
                Console.WriteLine("--------------------------------------------------------");
                foreach (var item in FoodIndex)
                {
                    Console.WriteLine(string.Format("{0} {1} {2}", item.Key, item.Value.Item2, item.Value.Item1));
                }
                Console.WriteLine("--------------------------------------------------------");
                Console.WriteLine("EXERCISE INDEX");
                Console.WriteLine("--------------------------------------------------------");
                foreach (var item in ExerciseIndex)
                {
                    Console.WriteLine(string.Format("{0} {1} {2}", item.Key, item.Value.Item2, item.Value.Item1));
                }
            }
        }
        
    }
}
