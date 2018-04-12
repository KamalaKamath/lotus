using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SugarSimulator
{
    public class SimpleSimulator
    {
        private static readonly string OUTPUT_FOLDER = ConfigurationManager.AppSettings["OUTPUT_FOLDER"];
        private const int BASE_GLUCOSE = 80;
        private const int LINEAR_DOWN = 60;
        private const int GLYCATION = 150;
        private const string FOOD = "food";
        private const string EXERCISE = "exercise";

        /*
          A Sugar simulator program will instantiate a type of sugar simulator and will save and output the results.
          Each simualator can vary its calculation methods and might provide various data points.
          But each should definitely implement interfaces
          Init() - Initialize all the dependency componenent mainly the food and exercise index database.
          Simulate() - Takes a set of food in takes by time and exercise done by time and outputs the sugar levels for the entire day
          PrintIndex - Displays the food and exercise index in the console.
         */
        public static bool Init()
        {

            /*We will be initializing all the components. Sucess is determined by the successful init of all components */
            Database.Init();
            return Database.Success;
        }

        public static void Simulate(List<Data> foodList, List<Data> exerciseList, int counter)
        {
            if (foodList == null)
            {
                Console.WriteLine("NOTHING to simulate. NO food input!");
                Console.WriteLine("-----------------------------------------------------------------");
            }
            else
            {
                Dictionary<int, Tuple<double, int>> glucose = new Dictionary<int, Tuple<double, int>>();

                var food = new Dictionary<int, int>();
                var exercise = new Dictionary<int, int>();

                var times = foodList.Select(f => f.Hour).Union(exerciseList.Select(e => e.Hour)).ToList();
                times.Sort();
                var min = times.First();
                var max = times.Last() + 2;

                InitData(food, foodList, FOOD);
                InitData(exercise, exerciseList, EXERCISE);

                Console.WriteLine(string.Format("Simulating Sugar Levels for the day with the {0} Food(s) and {1} Exercise(s) sets of inputs", foodList.Count, exerciseList.Count));
                Console.WriteLine("----------------------------------------------------------------------------------------");

                /*Init Base Glucose level for the day*/
                glucose.Add(0, Tuple.Create((double)BASE_GLUCOSE, 0));

                var lastgly = 0;

                for (int t = 1; t < 24; t++)
                {
                    var f = 0.0;
                    if ((t - 2) > 0)
                    {
                        if (food[t - 2] == 0 && food[t - 1] == 0 && exercise[t - 1] == 0)
                        {
                            f = (glucose[t - 1].Item1 - LINEAR_DOWN) >= BASE_GLUCOSE ? ((-1) * LINEAR_DOWN) : (-1) * Math.Abs(BASE_GLUCOSE - glucose[t - 1].Item1);
                        }
                        else
                        {
                            f = (0.5 * food[t - 2]) + (0.5 * food[t - 1]) - exercise[t - 1];
                        }
                    }
                    else
                    {
                        f = (0.5 * food[t - 1]) - exercise[t - 1];
                    }

                    var g = (glucose[t - 1].Item1 + f);

                    lastgly = lastgly + ((g > 150) ? 60 : 0);  /*Glycation Index  */
                        
                   glucose.Add(t, Tuple.Create(g, lastgly));
                    
                }
                SaveResults(glucose, min, max, counter);

            }
         }

        public static void PrintIndex()
        {
            Database.PrintIndex();
        }

        #region Private
        private static void InitData(Dictionary<int, int> lookup, List<Data> dataList, string type)
        {
            for (var i = 0; i < 24; i++)
            {
                lookup.Add(i, 0);
            }
            var toUpdate = dataList
            .Where(c => lookup.ContainsKey(c.Hour))
            .Select(c => new KeyValuePair<int, int>(c.Hour, c.Index));

            foreach (var kv in toUpdate)
            {
                lookup[kv.Key] = lookup[kv.Key] + (type == FOOD ? Database.FoodIndex[kv.Value].Item1 : Database.ExerciseIndex[kv.Value].Item1);
            }
        }

        private static void SaveResults(Dictionary<int, Tuple<double, int>> result, int min, int max, int counter)
        {
            /*Print it to the console */
            foreach (var item in result)
            {   if (item.Key >= min && item.Key <= max)
                {
                    Console.WriteLine(string.Format("{0},{1} {2}", item.Key, item.Value.Item1.ToString(), item.Value.Item2.ToString()));
                }
            }

            /* Save the results to a file csv format, so can use R and do plot*/
            var content = new StringBuilder();

            content.AppendLine("HOUR, SUGAR, GLYCATION");

            foreach (var item in result)
            {
                if (item.Key >= min && item.Key <= max)
                {
                    content.AppendLine(string.Format("{0},{1},{2}", item.Key, item.Value.Item1.ToString(), item.Value.Item2.ToString()));
                }
            }

            File.WriteAllText(string.Format("{0}Result{1}.csv ", OUTPUT_FOLDER, counter), content.ToString());
        }

#endregion

    }
}
