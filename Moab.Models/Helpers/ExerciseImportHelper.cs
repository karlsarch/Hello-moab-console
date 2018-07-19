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

        enum ExerciseCSVColumns
        {
            ExerciseCode, Name, CDT_Class, CDT_AtHome,
            IsMovementDataCollected, UnitTarget, HintEasier, HintHarder, Hint1,
            Hint2, MDT_Class, MDT_AtHome, OldCode, Name_animationFile,
            Old_Name_animationFile
        }
        const int numberOfColumns = 14;

        #endregion

        #region Public Methods

        /// <summary>
        ///     Primary public function from this class.
        ///     <paramref name="exercises">
        ///         Existing collection of exercises (can be empty) to be
        ///         updated or added to.
        ///     </paramref>
        ///     <paramref name="importCSV">
        ///         String version of the .CSV file imported.
        ///     </paramref>
        ///     <return>
        ///         Returns the update collection of exercises.
        ///     </return>
        ///     <tag status="In-Progress/Compiles"></tag>
        /// </summary>
        public ICollection<Exercise> Import(string importCSV, ICollection<Exercise> exercises)
        {
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
                    AddNewExercise(exercises, line);
                }
                else
                {
                    UpdateExercise(exercise, line);
                }
            }
            return exercises;
        }

        #endregion

        #region Debugging Code
        #if DEBUG

        /// <summary>
        ///     Public Accessor for SplitCSVLine Function
        ///     <paramref name="Line">
        ///         The line inputted in the form of a string
        ///         (see below to LineSplit function)
        ///     </paramref>
        ///     <return>
        ///         Returns an array of strings made out of the line
        ///     </return>
        ///     <tag status=Complete></tag>
        /// </summary>
        public string[] TestLineSplit(string Line)
        {
            return SplitCSVLine(Line);
        }

        /// <summary>
        ///     Public Accessor for SplitCSVInput Function
        ///     <paramref name="input">
        ///         The string based on the CSV file to be passed in
        ///     </paramref>
        ///     <return>
        ///         A list of arrays of strings will be easy to work with
        ///     </return>
        ///     <tag status=Complete></tag>
        /// </summary>
        public List<string[]> TestInputProcessing(string input)
        {
            return SplitCSVInput(input);
        }

        /// <summary>
        ///     Public Accessor for the IsHeaderValid Function
        ///     <paramref name="header">
        ///         The string based on the CSV file to be passed in
        ///     </paramref>
        ///     <return>
        ///         a boolean that will be true if the header is formatted
        ///         correctly and false if not.
        ///     </return>
        ///     <tag status=Complete></tag>
        /// </summary>
        public bool TestIsHeaderValid(string header)
        {
            return IsHeaderValid(header);
        }

        /// <summary>
        ///     Test code for FindNumHints function
        /// </summary>
        /// <param name="Line">
        ///     String format of input (should be one line of the CSV)
        /// </param>
        /// <returns>
        ///     Returns the number of hints in the line
        /// </returns>
        /// <tag status="Complete"></tag>
        public int TestnumHints(string Line)
        {
            string[] LineSplit = SplitCSVLine(Line);
            return findnumhints(LineSplit);
        }

        /// <summary>
        ///     Test code for the RefreshHint function.
        /// </summary>
        /// <param name="Line">
        ///     The CSV string that is input
        /// </param>
        /// <param name="exercise">
        ///     The exercise passed in to get its hints refreshed
        /// </param>
        /// <returns>
        ///     Returns the exercise with its hints refreshed from line
        /// </returns>
        /// <tag status=Complete></tag>
        public Exercise TestHints(Exercise exercise, string Line)
        {
            string[] LineSplit = SplitCSVLine(Line);
            RefreshHints(exercise, LineSplit);
            return exercise;
        }

#endif
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
        protected bool IsHeaderValid(string CSVInput)
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
            exercises.Add(exercise);
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
        ///     Splits the CSV Input into a list of arrays of strings
        ///     <paramref name="CSVInput">
        ///         The string based on the CSV file to be passed in
        ///     </paramref>
        ///     <return>
        ///         A list of arrays of strings will be easy to work with
        ///     </return>
        ///     <tag status=Complete></tag>
        /// </summary>
        protected List<string[]> SplitCSVInput(string CSVInput)
        {
            // Create LineList
            var LineList = new List<string[]>();
            int iterator = 0;
            do
            {
                string[] line = new string[numberOfColumns];
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
                line = SplitCSVLine(temp);
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
        private void RefreshHints(Exercise exercise, string[] CSVLine)
        {
            exercise.ExerciseHints.Clear();
            for (int i = 0; i < findnumhints(CSVLine); i++)
            {
                var hint = new ExerciseHint();
                hint.Id = exercise.Id;
                hint.ExerciseID = exercise.Id; //Is this the right connection?
                hint.Text = CSVLine[(int)ExerciseCSVColumns.Hint1 + i];
                exercise.ExerciseHints.Add(hint);
            }
        }

        /// <summary>
        ///     Finds the number of hints (not including easierhint or
        ///     harderhint) in the CSVLine
        /// </summary>
        /// <param name="CSVLine">
        ///     The input string array
        /// </param>
        /// <returns>
        ///     Returns the number of hints (not including easierhint or
        ///     harderhint) in the string array
        /// </returns>
        /// <tag status="In-Progress/Requires Testing"></tag>
        private int findnumhints(string[] CSVLine)
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
        ///     Converts "Y" or "y" into true and everything else into false
        ///     Helper function for UpdateExercise()
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
        ///     Splits each line into an array of strings
        ///     Helper function to SplitCSVInput
        ///     TODO: Implementation for non-fixed number of hints
        ///     <paramref name="CSVLine">
        ///         The line inputted in the form of a string
        ///         (see below to LineSplit function)
        ///     </paramref>
        ///     <return>
        ///         Returns an array of strings made out of the line
        ///     </return>
        ///     <tag status="In-Progress/Compiles"></tag>
        /// </summary>
        private string[] SplitCSVLine(string CSVLine)
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

        #endregion
    }
}
