using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nymbl.Models.POCO;



namespace Moab.Models.Helpers
{
    public class ExerciseImportHelper
    {
        enum ExerciseCSVColumns { ExerciseCode, Name, CDT_Class, CDT_AtHome, IsMovementDataCollected, UnitTarget, HintEasier, HintHarder, Hint1, Hint2, MDT_Class, MDT_AtHome, OldCode, Name_animationFile, Old_Name_animationFile }

        #region Members

        #endregion

        #region Public Methods

        public ICollection<Exercise> Import(string importCSV, ICollection<Exercise> exercises)
        {
            List<string[]> ImportList = SplitCSVInput(importCSV);
            //TODO: add support in the SplitCSVInput method for checking header
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

        #region test code for private methods  **REMOVE FROM PRODUCTION CODE**

        public string[] TestLineSplit(string Line)
        {
            return SplitCSVLine(Line);
        }

        public List<string[]> TestInputProcessing(string input)
        {
            return SplitCSVInput(input);
        }

        public bool TestIsHeaderValid(string header)
        {
            return IsHeaderValid(header);
        }

        #endregion

        #region Private / Protected Methods
        protected bool IsHeaderValid(string header)
        {
            string checkHeader = "ExerciseCode,Name,CDT_Class,CDT_AtHome,IsMovementDataCollected,UnitTarget,HintEasier,HintHarder,Hint1,Hint2,MDT_Class,MDT_AtHome,OldCode,Name_animationFile,Old_Name_animationFile";
            if (header == checkHeader)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected Exercise FindExtantExerciseInCollection(ICollection<Exercise> exercises, string exerciseCode)
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

        protected void UpdateExercise(Exercise exercise, string[] updateCSV)
        {
            //TODO: add support for  generic hint collection
            exercise.ExerciseCode = updateCSV[(int)ExerciseCSVColumns.ExerciseCode];
            exercise.Name = updateCSV[(int)ExerciseCSVColumns.Name];
            exercise.EasierHint = updateCSV[(int)ExerciseCSVColumns.HintEasier];
            exercise.HarderHint = updateCSV[(int)ExerciseCSVColumns.HintHarder];
            exercise.HasRepetitionTarget = ConvertYNtoBool(updateCSV[(int)ExerciseCSVColumns.UnitTarget]);
            exercise.DateLastUpdated = DateTime.Now;
        }

        protected void AddNewExercise(ICollection<Exercise> exercises, string[] newCSV)
        {
            Exercise exercise = new Exercise();
            UpdateExercise(exercise, newCSV);
            exercises.Add(exercise);
        }

        // Converts Y into true and N into false
        // Helper function for UpdateExercise()
        protected bool ConvertYNtoBool(string input)
        {
            if (input.ToUpper() == "Y")
            {
                return true;
            }
            else if (input.ToUpper() == "N")
            {
                return false;
            }
            return false;
        }

        // Converts the input string into a
        protected List<string[]> SplitCSVInput(string CSVInput)
        {
            // Create LineList
            List<string[]> LineList = new List<string[]>();
            int iterator = 0;
            do
            {
                string[] line = new string[14];
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
            LineList.RemoveRange(0, 2); //should this be (0,1) because we only want to delete the first two lines?

            // Return edited list
            return LineList;
        }

        // Splits each line into an array of strings
        // Helper function to SplitCSVInput
        // TODO: Implementation for non-fixed number of hints
        private string[] SplitCSVLine(string CSVLine)
        {
            int numberOfColumns = 14;
            string[] Line = new string[numberOfColumns]; 
            int iterator = 0;
            for (int i = 0; i < 14; i++)
            {
                if (CSVLine[iterator] == '\"')
                {
                    int iteratorNext = CSVLine.IndexOf('\"', iterator);
                    if (iteratorNext == -1)
                    {
                        throw new FormatException("Invalid Format of CSV Input");
                    }
                    string temp = CSVLine.Substring(iterator, iteratorNext - iterator);
                    Line[i] = temp;
                    iterator = iteratorNext + 2;
                }
                else
                {
                    int iteratorNext = CSVLine.IndexOf(',', iterator);
                    if (iteratorNext == -1)
                    {
                        throw new FormatException("Invalid Format of CSV Input");
                    }
                    string temp = CSVLine.Substring(iterator, iteratorNext - iterator);
                    Line[i] = temp;
                    iterator = ++iteratorNext;
                }
            }
            return Line;
        }

        #endregion
    }
}
