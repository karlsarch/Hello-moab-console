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
        #region Members

        private ICSVProcessor _csvProcessor;

        #endregion

        #region Constructors

        public ExerciseExportHelper(ICSVProcessor processor)
        {
            _csvProcessor = processor;
        }

        public ExerciseExportHelper()
        {
            _csvProcessor = new CSVProcessor();
        }

        #endregion

        #region Public Methods
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
            var exportCSVStringBuilder = new StringBuilder(_csvProcessor.GenerateHeader(FindMaxNumHints(exercises)));
            foreach (Exercise i in exercises)
            {
                exportCSVStringBuilder.Append(MakeCSVLine(i));
            }
            string exportCSV = exportCSVStringBuilder.ToString();
            return exportCSV;
        }

        #endregion

        #region Private/Protected Methods
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

        #endregion
    }
}
