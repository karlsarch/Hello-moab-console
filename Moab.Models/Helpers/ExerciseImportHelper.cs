﻿using System;
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
        enum ExerciseCSVColumns { ExerciseCode, Name, CDT_Class, CDT_AtHome,
            IsMovementDataCollected, UnitTarget, HintEasier, HintHarder, Hint1,
            Hint2, MDT_Class, MDT_AtHome, OldCode, Name_animationFile,
            Old_Name_animationFile }

        #region Members

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
                Exercise exercise = FindExtantExerciseInCollection(ref exercises, line[0]);
                if (exercise == null)
                {
                    AddNewExercise(ref exercises, line);
                }
                else
                {
                    UpdateExercise(ref exercise, line);
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

#endif
        #endregion

        #region Private / Protected Methods

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
        protected Exercise FindExtantExerciseInCollection(ref
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
        protected void UpdateExercise(ref Exercise exercise, string[] updateCSV)
        {
            //TODO: add support for  generic hint collection
            exercise.ExerciseCode = updateCSV[(int)ExerciseCSVColumns.ExerciseCode];
            exercise.Name = updateCSV[(int)ExerciseCSVColumns.Name];
            exercise.EasierHint = updateCSV[(int)ExerciseCSVColumns.HintEasier];
            exercise.HarderHint = updateCSV[(int)ExerciseCSVColumns.HintHarder];
            exercise.HasRepetitionTarget = ConvertYNtoBool(updateCSV[(int)ExerciseCSVColumns.UnitTarget]);
            exercise.DateLastUpdated = DateTime.Now;
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
        protected void AddNewExercise(ref ICollection<Exercise> exercises, string[] newCSV)
        {
            Exercise exercise = new Exercise();
            UpdateExercise(ref exercise, newCSV);
            exercises.Add(exercise);
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
        protected bool ConvertYNtoBool(string input)
        {
            return (input.ToUpper() == "Y");
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
            if (LineList.Count == 0)
            {
                throw new FormatException("Invalid Format of CSV Input");
            }

            // Delete Header and Empty Row
            LineList.RemoveRange(0, 2);

            // Return edited list
            return LineList;
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
            var Line = new string[LineList.Count];
            for (int i = 0; i <LineList.Count; i++)
            {
                Line[i] = LineList[i];
            }
            return Line;
        }

        #endregion
    }
}
