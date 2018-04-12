using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace SugarSimulator.DataProvider
{
    public class FileDataProvider
    {
        private static readonly string FILE_EXERCISE = ConfigurationManager.AppSettings["FILE_EXERCISE"];
        private static readonly string FILE_FOOD = ConfigurationManager.AppSettings["FILE_FOOD"];
        
        public void LoadFoodIndex(ref Dictionary<int, Tuple<int, string>> dict)
        {
     
            dict = new Dictionary<int, Tuple<int, string>>();
            TextFieldParser parser = new TextFieldParser(FILE_FOOD);

            parser.HasFieldsEnclosedInQuotes = true;
            parser.SetDelimiters(",");
            parser.HasFieldsEnclosedInQuotes = true;
            string[] fields;
            bool skipHeader = true;
            while (!parser.EndOfData)
            {
                fields = parser.ReadFields();
                if (!skipHeader)
                {
                    dict.Add(Int32.Parse(fields[0]), Tuple.Create(Int32.Parse(fields[2]), fields[1]));
                }
                skipHeader = false;
            }
            parser.Close();
        }
        public void LoadExerciseIndex(ref Dictionary<int, Tuple<int, string>> dict)
        {
         
            dict = new Dictionary<int, Tuple<int, string>>();
            TextFieldParser parser = new TextFieldParser(FILE_EXERCISE);

            parser.HasFieldsEnclosedInQuotes = true;
            parser.SetDelimiters(",");
            parser.HasFieldsEnclosedInQuotes = true;
            string[] fields;
            bool skipHeader = true;
            while (!parser.EndOfData)
            {
                fields = parser.ReadFields();
                if (!skipHeader)
                {
                    dict.Add(Int32.Parse(fields[0]), Tuple.Create(Int32.Parse(fields[2]), fields[1]));
                }
                skipHeader = false;
            }
            parser.Close();

        }
    }
}
