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
        #region Members



        // Easier and Harder hints are "special" and not considered Hints per se, because they don't go into the ExerciseHints collection.
        // So
        private readonly int NumNonHintColumns;
        private readonly int NumPreHintColumns;
        private ICSVProcessor _csvProcessor;
        private int _numberOfHints;

        #endregion

        #region Constructors

        public ExerciseImportHelper(ICSVProcessor processor)
        {
            _csvProcessor = processor;
            var preVals = Enum.GetValues(typeof(ExerciseCSVPreHintColumns));
            var postVals = Enum.GetValues(typeof(ExerciseCSVPostHintColumns));
            NumNonHintColumns = preVals.Length + postVals.Length;
            NumPreHintColumns = preVals.Length;
        }

        public ExerciseImportHelper()
        {
            _csvProcessor = new CSVProcessor();
            var preVals = Enum.GetValues(typeof(ExerciseCSVPreHintColumns));
            var postVals = Enum.GetValues(typeof(ExerciseCSVPostHintColumns));
            NumNonHintColumns = preVals.Length + postVals.Length;
            NumPreHintColumns = preVals.Length;
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Primary public function from this class; imports data into Exercises, but does not
        ///     remove any previously existing Exercises, even if they are not present in the CSV input
        ///     <paramref name="exercises">
        ///         Existing collection of exercises (can be empty) to be updated or added to.
        ///     </paramref>
        ///     <paramref name="importCSV">
        ///         multi-line string representing the data to be imported in .csv format.
        ///     </paramref>
        ///     <return>
        ///         The revised collection of exercises.
        ///     </return>
        ///     <tag status="In-Progress/Compiles"></tag>
        /// </summary>
        public ICollection<Exercise> ImportWithoutDelete(string importCSV, ICollection<Exercise> exercises)
        {
            if (importCSV == null || exercises == null || !importCSV.Contains(Environment.NewLine))
            {
                throw new ArgumentException("Arguments must not be null and the input string must contain at least two lines.");
            }

            if (!IsHeaderValid(importCSV.Substring(0, importCSV.IndexOf(Environment.NewLine))))
            {

                var header = _csvProcessor.GenerateHeader(_numberOfHints);

                throw new FormatException($"Input CSV has unexpected column format. Expected format: '{header}'. " +
                                          $"Note that a variable number of Hint columns are allowed.");
            }

            List<string[]> importList = SplitCSVInput(importCSV);

            foreach (var line in importList)
            {
                Exercise exercise = FindExtantExerciseInCollection(exercises, line[(int)ExerciseCSVPreHintColumns.ExerciseCode]);
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
        /// Imports the CSV string to a new collection, tracking updates and new Exercises. Untouched objects
        /// remain in the originally passed in collection, indicating which can be marked for deletion.
        /// </summary>
        /// <param name="importCSV"></param>
        /// <param name="exercises"></param>
        /// <returns>The new collection of updated and added exercises</returns>
        public ICollection<Exercise> ImportWithDelete(string importCSV, ICollection<Exercise> exercises)
        {
            ICollection<Exercise> updatedAndNewExercises = new HashSet<Exercise>();
            List<string[]> ImportList = SplitCSVInput(importCSV);
            if (!IsHeaderValid(importCSV.Substring(0, importCSV.IndexOf(Environment.NewLine))))
            {
                var header = _csvProcessor.GenerateHeader(_numberOfHints);

                throw new FormatException($"Input CSV has unexpected column format. Expected format: '{header}'. " +
                                          $"Note that a variable number of Hint columns are allowed.");
            }

            foreach (string[] line in ImportList)
            {
                Exercise exercise = FindExtantExerciseInCollection(exercises, line[(int)ExerciseCSVPreHintColumns.ExerciseCode]);
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
        internal bool IsHeaderValid(string headerLine)
        {

            try
            {
                _numberOfHints = FindNumHints(headerLine.Split(','));
                string expectedHeader = _csvProcessor.GenerateHeader(_numberOfHints);
                // Removes any unnecesary columns from the end of header
                string headerWeCare = headerLine.Substring(0, expectedHeader.Length);
                // Returns true if they are the same excluding case differences
                return headerWeCare.ToLower() == expectedHeader.ToLower();
            }
            catch (Exception)
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
            exercise.ExerciseCode = updateCSV[(int)ExerciseCSVPreHintColumns.ExerciseCode];
            exercise.Name = updateCSV[(int)ExerciseCSVPreHintColumns.Name];
            exercise.HasRepetitionTarget = ConvertYNtoBool(updateCSV[(int)ExerciseCSVPreHintColumns.UnitTarget]);
            exercise.EasierHint = updateCSV[(int)ExerciseCSVPreHintColumns.HintEasier];
            exercise.HarderHint = updateCSV[(int)ExerciseCSVPreHintColumns.HintHarder];
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
        ///     <paramref name="csvInput">
        ///         The string based on the CSV file to be passed in
        ///     </paramref>
        ///     <return>
        ///         A list of arrays of strings will be easy to work with
        ///     </return>
        ///     <tag status=Complete></tag>
        /// </summary>
        internal List<string[]> SplitCSVInput(string csvInput)
        {
            if (string.IsNullOrEmpty(csvInput))
            {
                throw new ArgumentException("input csv string cannot be null.");
            }
            // Create LineList
            var lineList = new List<string[]>();

            var lines = csvInput.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Count() < 2)
            {
                throw new ArgumentException("Invalid input; no records found");
            }

            foreach (var line in lines)
            {
                lineList.Add(SplitCSVLine(line));
            }

            // Delete Header
            lineList.RemoveAt(0);

            // Delete Each Empty Row
            for (int i = 0; i < lineList.Count; i++)
            {
                if (lineList[i][0] == "")
                {
                    lineList.RemoveAt(i);
                }
            }

            // Return edited list
            return lineList;
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
            for (int i = 0; i < _numberOfHints; i++)
            {
                var hint = new ExerciseHint()
                {
                    Text = CSVLine[(int) NumPreHintColumns + i]
                };
                exercise.ExerciseHints.Add(hint);
            }
        }

        /// <summary>
        ///     Finds the number of hints (not including easierhint or harderhint) in the CSVLine
        /// </summary>
        /// <param name="headerLine">
        ///     The input string array
        /// </param>
        /// <returns>
        ///     Returns the number of hints (not including easierhint or harderhint) in the string array
        /// </returns>
        /// <tag status="In-Progress/Requires Testing"></tag>
        internal int FindNumHints(string[] headerLine)
        {
            var index = headerLine.ToList().IndexOf("MDT_Class"); // This is the first column expected after hints
            if (index >= NumPreHintColumns)
            {
                return index - NumPreHintColumns;
            }
            else
            {
                throw new FormatException("Invalid csv Format");
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
            if (input.Length != 1)
            {
                throw new FormatException($"Expected boolean Y or N but encountered {input}.");
            }
            return (input.ToUpper() == "Y");
        }

        /// <summary>
        ///     Splits each line into an array of strings Helper function to SplitCSVInput
        ///     <paramref name="csvLine">
        ///         The line inputted in the form of a string
        ///         (see below to LineSplit function)
        ///     </paramref>
        ///     <return>
        ///         Returns an array of strings made out of the line
        ///     </return>
        ///     <tag status="In-Progress/Compiles"></tag>
        /// </summary>
        internal string[] SplitCSVLine(string csvLine)
        {
            var rawSplit = csvLine.Split(new[] { ',' }, StringSplitOptions.None);
            var fields = new List<string>();
            for (int i=0; i < rawSplit.Length; i++)
            {
                var field = rawSplit[i];
                if (!string.IsNullOrEmpty(field) && field[0] == '\"')
                {
                    while (rawSplit[i][rawSplit[i].Length-1] != '\"')
                    {
                        // We've got a quoted field, which needs to be combined. So we'll
                        // force the loop ahead by one and combine the field.
                        i++;
                        if (i >= rawSplit.Length)
                        {
                            throw new FormatException("Missing closing quote.");
                        }
                        field += $",{rawSplit[i]}";
                    }
                }
                fields.Add(field.Trim(' ', '"'));
            }

            return fields.ToArray();

            //char[] splitters = { ',', '\"' };
            //string[] temp = csvLine.Split(splitters);
            //int j = 0;
            //foreach (string rawField in temp)
            //{
            //    if (!string.IsNullOrEmpty(rawField))
            //    {
            //        if (rawField[0] == '\"')
            //        {
            //            temp[j] = temp[j] + temp[j + 1];
            //            temp[j + 1] = null;
            //        }
            //        j++;
            //    }
            //    rawField.Trim();

            //}
            //var LineList = new List<string>();
            //foreach (string i in temp)
            //{
            //    if (!(i == null))
            //    {
            //        LineList.Add(i);
            //    }
            //}
            //var line = new string[LineList.Count];
            //for (int i = 0; i <LineList.Count; i++)
            //{
            //    line[i] = LineList[i];
            //}
            //return line;
        }

        #endregion
    }
}
