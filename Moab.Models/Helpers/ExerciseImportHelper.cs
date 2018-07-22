using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nymbl.Models.POCO;


namespace Moab.Models.Helpers
{
    /// <summary>
    ///     Helps import a .csv file into a collection of exercises.
    /// </summary>
    /// <tag status=In-Progress/Compiles></tag>
    public class ExerciseImportHelper
    {
        #region Members

        public enum ExerciseCSVColumns
        {
            ExerciseCode, Name, CDT_Class, CDT_AtHome,
            IsMovementDataCollected, UnitTarget, HintEasier, HintHarder,
            Hint1, Hint2,
            MDT_Class, MDT_AtHome, OldCode, Name_animationFile,
            Old_Name_animationFile
        }
        private const int NumNonHintColumns = 13;

        #endregion

        #region Public Methods

        /// <summary>
        ///     Primary public function from this class.
        ///     <paramref name="exercises">
        ///         Existing collection of exercises (can be empty) to be updated or added to.
        ///     </paramref>
        ///     <paramref name="importCSV">
        ///         multi-line string representing the data to be imported in .csv format.
        ///     </paramref>
        ///     <return>
        ///         The updated collection of exercises.
        ///     </return>
        ///     <tag status="In-Progress/Compiles"></tag>
        /// </summary>
        public ICollection<Exercise> ImportNoDelete(string importCSV, ICollection<Exercise> exercises)
        {
            if (importCSV == null || exercises == null)
            {
                throw new ArgumentException("Arguments must not be null.");
            }

            List<string[]> ImportList = SplitCSVInput(importCSV);
            if (!IsHeaderValid(importCSV))
            {
                var values = Enum.GetNames(typeof(ExerciseCSVColumns));
                var sb = new StringBuilder($"{values[0]}");
                for (int i = 1; i < values.Length; i++ )
                {
                    sb.Append($",{values[i]}");
                }

                throw new FormatException($"Input CSV has unexpected column format. Expected format: '{sb.ToString()}'. Note that variable number of Hint columns are allowed.");
            }

            foreach (var line in ImportList)
            {
                Exercise exercise = FindExtantExerciseInCollection(exercises, line[0]);
                if (exercise == null)
                {
                    AddNewExercise(exercises, line);
                }
                else
                {
                    UpdateExercise(exercise, line);
                }
            }
            return exercises;
        }

        /// <summary>
        /// Imports the CSV string to a new collection and leaves the old one 
        /// with just the objects to be deleted.
        /// </summary>
        /// <param name="importCSV"></param>
        /// <param name="exercises"></param>
        /// <returns>The new collection of updated and added exercises</returns>
        public ICollection<Exercise> ImportWithDelete(string importCSV, ICollection<Exercise> exercises)
        {
            ICollection<Exercise> updatedAndNewExercises = new HashSet<Exercise>();
            List<string[]> ImportList = SplitCSVInput(importCSV);
            if (!IsHeaderValid(importCSV))
            {
                throw new FormatException("Header is in improper format.");
            }

            foreach (string[] line in ImportList)
            {
                Exercise exercise = FindExtantExerciseInCollection(exercises, line[0]);
                if (exercise == null)
                {
                    AddNewExercise(updatedAndNewExercises, line);
                }
                else
                {
                    updatedAndNewExercises.Add(exercise);
                    exercises.Remove(exercise);
                    UpdateExercise(exercise, line);
                }
            }
            return updatedAndNewExercises;
        }

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

        #endregion


        #region Protected Methods

        /// <summary>
        ///     Checks if the Header of the CSV Input is in valid format
        /// </summary>
        /// <param name="CSVInput">
        ///     The Input CSV File in the form of one string
        /// </param>
        /// <returns>
        ///     Returns true if the header is in proper format, false otherwise
        /// </returns>
        /// <tag status=Complete></tag>
        internal bool IsHeaderValid(string CSVInput)
        {
            CSVInput.Trim();
            string[] split = CSVInput.Split('\n');
            return CheckHeader(ref split[0]);
        }

        /// <summary>
        ///     Helper Function to IsHeaderValid
        ///     Checks if the Header is actually valid after IsHeaderValid
        ///         splits the input
        /// </summary>
        /// <param name="header">
        ///     The Header of the file only
        /// </param>
        /// <returns>
        ///     Returns true if header is valid, false if not
        /// </returns>
        /// <tag status="Complete"></tag>
        private bool CheckHeader(ref string header)
        {
            const string checkHeader = "ExerciseCode,Name,CDT_Class,CDT_AtHome," +
                "IsMovementDataCollected,UnitTarget,HintEasier,HintHarder," +
                "Hint1,Hint2,MDT_Class,MDT_AtHome,OldCode," +
                "Name_animationFile,Old_Name_animationFile";
            try
            {
                // Removes any unnecesary columns from the end of header
                string headerWeCare = header.Substring(0, checkHeader.Length);
                // Returns true if they are the same excluding case differences
                return headerWeCare.ToLower() == checkHeader.ToLower();
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }

        }

        /// <summary>
        ///     Finds the exercise in the collection by code
        /// </summary>
        /// <param name="exercises">
        ///     The collection of exercises inputted from other code
        /// </param>
        /// <param name="exerciseCode">
        ///     The Code of the exercise that we're looking for
        /// </param>
        /// <returns>
        ///     Returns the exercise that matches the code if it exists
        ///     Returns null if not
        /// </returns>
        /// <tag status=NeedsTest></tag>
        protected Exercise FindExtantExerciseInCollection(
            ICollection<Exercise> exercises, string exerciseCode)
        {
           foreach (Exercise exercise in exercises)
            {
                if (exercise.ExerciseCode == exerciseCode)
                {
                    return exercise;
                }
            }
            return null;
        }

        /// <summary>
        ///     Updates the necessary parts of each exercise if it already exists
        /// </summary>
        /// <param name="exercise">
        ///     The exercise object to be updated
        /// </param>
        /// <param name="updateCSV">
        ///     The array of strings that corresponds to each exercise
        /// </param>
        /// <tag status=In-Progress/Compiles>Perhaps Requires Reference
        /// Parameter</tag>
        protected void UpdateExercise(Exercise exercise, string[] updateCSV)
        {
            exercise.ExerciseCode = updateCSV[(int)ExerciseCSVColumns.ExerciseCode];
            exercise.Name = updateCSV[(int)ExerciseCSVColumns.Name];
            exercise.EasierHint = updateCSV[(int)ExerciseCSVColumns.HintEasier];
            exercise.HarderHint = updateCSV[(int)ExerciseCSVColumns.HintHarder];
            exercise.HasRepetitionTarget = ConvertYNtoBool(updateCSV[(int)ExerciseCSVColumns.UnitTarget]);
            exercise.DateLastUpdated = DateTime.Now;
            RefreshHints(exercise, updateCSV);
        }

        /// <summary>
        ///     Adds a new exercise to the collection of exercises
        /// </summary>
        /// <param name="exercises">
        ///     The exercise object to be updated
        /// </param>
        /// <param name="newCSV">
        ///     The array of strings that corresponds to each exercise
        /// </param>
        /// <tag status=In-Progress/Compiles>Perhaps Requires Reference
        /// Parameter</tag>
        protected void AddNewExercise(ICollection<Exercise> exercises, string[] newCSV)
        {
            var exercise = new Exercise();
            UpdateExercise(exercise, newCSV);
            exercise.DateCreated = DateTime.Now;
            exercises.Add(exercise);
        }

        /// <summary>
        ///     Splits the CSV Input into a list of arrays of strings
        ///     <paramref name="CSVInput">
        ///         The string based on the CSV file to be passed in
        ///     </paramref>
        ///     <return>
        ///         A list of arrays of strings will be easy to work with
        ///     </return>
        ///     <tag status=Complete></tag>
        /// </summary>
        internal List<string[]> SplitCSVInput(string CSVInput)
        {
            // Create LineList
            var LineList = new List<string[]>();
            int iterator = 0;
            do
            {
                int iteratorNext = CSVInput.IndexOf('\n', iterator);
                string temp;
                if (iteratorNext == -1)
                {

                    temp = CSVInput.Substring(iterator, CSVInput.Length - iterator);
                }
                else
                {
                    temp = CSVInput.Substring(iterator, iteratorNext - iterator);
                }
                string[] line = SplitCSVLine(temp);
                LineList.Add(line);
                iterator = ++iteratorNext;
            }
            while (iterator > 0);
            if (LineList.Count <= 1)
            {
                throw new FormatException("Invalid Format of CSV Input");
            }

            // Delete Header
            LineList.RemoveAt(0);

            // Delete Each Empty Row
            for (int i = 0; i < LineList.Count; i++)
            {
                if (LineList[i][0] == "")
                {
                    LineList.RemoveAt(i);
                }
            }

            // Return edited list
            return LineList;
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

        #endregion

        #region Private Methods

        /// <summary>
        ///     Refreshes the hints to the exercise.  Helper function to
        ///     UpdateExercise.
        /// </summary>
        /// <param name="exercise">
        ///     The exercise passed in that I am to refresh the hints for.
        /// </param>
        /// <param name="updateCSV">
        ///     The processed CSV line that I am taking the hints from.
        /// </param>
        /// <tag status="In-Progress/Requires Testing"></tag>
        internal void RefreshHints(Exercise exercise, string[] CSVLine)
        {
            exercise.ExerciseHints.Clear();
            for (int i = 0; i < FindNumHints(CSVLine); i++)
            {
                var hint = new ExerciseHint()
                {
                    Text = CSVLine[(int)ExerciseCSVColumns.Hint1 + i]
                };
                exercise.ExerciseHints.Add(hint);
            }
        }

        /// <summary>
        ///     Finds the number of hints (not including easierhint or harderhint) in the CSVLine
        /// </summary>
        /// <param name="CSVLine">
        ///     The input string array
        /// </param>
        /// <returns>
        ///     Returns the number of hints (not including easierhint or harderhint) in the string array
        /// </returns>
        /// <tag status="In-Progress/Requires Testing"></tag>
        internal static int FindNumHints(string[] CSVLine)
        {
            const int numNonHintColumns = 13;
            if (CSVLine.Length >= numNonHintColumns)
            {
                return CSVLine.Length - numNonHintColumns;
            }
            else
            {
                throw new FormatException("Invalid File Format");
            }
        }

        /// <summary>
        ///     Converts "Y" or "y" into true and everything else into false Helper function for UpdateExercise()
        ///     <paramref name="input">
        ///         The input string (likely either Y or N)
        ///     </paramref>
        ///     <return>
        ///         Returns true if "Y" or "y", false otherwise
        ///     </return>
        ///     <tag status=Complete></tag>
        /// </summary>
        private bool ConvertYNtoBool(string input)
        {
            return (input.ToUpper() == "Y");
        }

        /// <summary>
        ///     Splits each line into an array of strings Helper function to SplitCSVInput
        ///     <paramref name="CSVLine">
        ///         The line inputted in the form of a string
        ///         (see below to LineSplit function)
        ///     </paramref>
        ///     <return>
        ///         Returns an array of strings made out of the line
        ///     </return>
        ///     <tag status="In-Progress/Compiles"></tag>
        /// </summary>
        internal string[] SplitCSVLine(string CSVLine)
        {
            char[] splitters = { ',', '\"' };
            string[] temp = CSVLine.Split(splitters);
            int j = 0;
            foreach (string i in temp)
            {
                i.Trim();
                if (i != "")
                {
                    if (i[0] == '\"')
                    {
                        temp[j] = temp[j] + temp[j + 1];
                        temp[j + 1] = null;
                    }
                    j++;
                }

            }
            var LineList = new List<string>();
            foreach (string i in temp)
            {
                if (!(i == null))
                {
                    LineList.Add(i);
                }
            }
            var line = new string[LineList.Count];
            for (int i = 0; i <LineList.Count; i++)
            {
                line[i] = LineList[i];
            }
            return line;
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
