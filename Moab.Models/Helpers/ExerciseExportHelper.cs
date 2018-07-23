using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nymbl.Models.POCO;

namespace Moab.Models.Helpers
{
    class ExerciseExportHelper
    {
        /// <summary>
        ///     Exports a string in .csv form given a collection of exercises.
        /// </summary>
        /// <param name="exercises">
        ///     The collection of exercises inputted
        /// </param>
        /// <returns>
        ///     Returns a string that can be written directly to a .csv file
        /// </returns>
        /// <tag status="Complete/Requires Testing"></tag>
        public string Export(ICollection<Exercise> exercises)
        {
            string ExportCSV = CreateHeader(FindMaxNumHints(exercises));
            foreach (Exercise i in exercises)
            {
                ExportCSV += MakeCSVLine(i);
            }
            return ExportCSV;
        }

        /// <summary>
        ///     Turns each exercise into a string that is the csv format of
        ///     the exercise
        /// </summary>
        /// <param name="exercise">
        ///     The exercise to be turned into a string
        /// </param>
        /// <returns>
        ///     A string that is the .csv format of the exercise including
        ///     the newline character
        /// </returns>
        /// <tag status=In-Progress/Compiles></tag>
        private string MakeCSVLine(Exercise exercise)
        {
            string CSVLine = string.Empty;
            CSVLine += exercise.ExerciseCode + ",";
            CSVLine += exercise.Name + ",";
            for (int i = 0; i < 3; i++) // for CDT_Class, CDT_AtHome, and IsMovementDataCollected
            {
                CSVLine += "Unknown,";
            }
            CSVLine += ConvertBooltoYN(exercise.HasRepetitionTarget) + ",";
            CSVLine += exercise.EasierHint + ",";
            CSVLine += exercise.HarderHint + ",";
            foreach (ExerciseHint hint in exercise.ExerciseHints)
            {
                CSVLine += hint.Text + ",";
            }
            for (int i = 0; i < 5; i++) // for MDT_Class, MDT_AtHome, OldCode, Name_animationFile, and Old_Name_animationFile
            {
                CSVLine += "Unknown,";
            }
            CSVLine += Environment.NewLine;
            return CSVLine;
        }

        /// <summary>
        ///     Returns a string that is a valid header and emty line for the
        ///     collection of exercises with the maximum number of hints
        ///     passed in.
        /// </summary>
        /// <param name="maxNumHints">
        ///     The maximum number of hints any exercise in this collection has
        /// </param>
        /// <returns>
        ///     A string that is the header for a .CSV file
        /// </returns>
        /// <tag status="Complete"></tag>
        protected string CreateHeader(int maxNumHints)
        {
            string CSV = "ExerciseCode, Name, CDT_Class, CDT_AtHome, IsMov" +
                "ementDataCollected, UnitTarget, HintEasier, HintHarder,";
            for (int i = 1; i <= maxNumHints; i++)
            {
                CSV += "Hint" + i.ToString() + ",";
            }
            CSV += "MDT_Class,MDT_AtHome,OldCode,Name_animationFile,Old_Nam" +
                "e_animationFile" + Environment.NewLine + ",,,,,,,,,,,,," +
                "," + Environment.NewLine;
            return CSV;
        }

        /// <summary>
        ///     Finds the maximum number of hints any exercise in the
        ///     collection has
        /// </summary>
        /// <param name="exercises">
        ///     The collection of exercises that we are checking the hints for
        /// </param>
        /// <returns>
        ///     The maximum number of hints that any of the exercises in the
        ///     collection has
        /// </returns>
        /// <tag status="Complete"></tag>
        internal static int FindMaxNumHints(ICollection<Exercise> exercises)
        {
            int maxNum = 0;
            foreach (Exercise ex in exercises)
            {
                int numHints = 0;
                foreach (ExerciseHint hint in ex.ExerciseHints)
                {
                    numHints++;
                }
                if (numHints > maxNum)
                {
                    maxNum = numHints;
                }
            }
            return maxNum;
        }

        /// <summary>
        ///     Converts anything that is true to "Y" and false to "N"
        /// </summary>
        /// <param name="value">
        ///     Boolean to convert
        /// </param>
        /// <returns>
        ///     "Y" if YN == true, false otherwise
        /// </returns>
        /// <tag status=Complete></tag>
        private string ConvertBooltoYN(bool value)
        {
            if (value)
            {
                return "Y";
            }
            return "N";
        }
    }
}
