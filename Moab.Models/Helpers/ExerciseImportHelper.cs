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

        public bool Import(string importCSV, ICollection<Exercise> exercises)
        {
            // TODO: Write the implementation!!!!!
            //In my method UpdateExercise, I pass in a string array as the CSV file. This implies
            //that at some point we create a method that splits the lines of CSV into arrays.
            //However, we can't do this using the split method because of the commas in some hints.
            Vector<string>(inputLines) = new Vector<string>;



            return false;
        }

        #endregion

        #region Private / Protected Methods
        protected bool IsHeaderValid(string header)
        {
            string checkHeader = "ExerciseCode, Name, CDT_Class, CDT_AtHome, IsMovementDataCollected, UnitTarget, HintEasier, HintHarder, Hint1, Hint2, MDT_Class, MDT_AtHome, OldCode, Name_animationFile, Old_Name_animationFile";
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

        // Converts Y into true and N into false
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

        protected string[] SplitCSVInput(string CSVInput)
        {

            List<string> LineList = new List<string>();

            for (int i = 0; i < /*TODO*/; i++)
            {

            }



            return null;
        }

        // Splits each line into an array of strings, usable later
        // Done, requires testing to confirm
        protected string[] SplitCSVLine(string CSVLine)
        {
            string[] Line = new string[14];
            int iterator = 0;
            for (int i = 0; i < 14; i++)
            {
                if (CSVLine[iterator] == '\"')
                {
                    int iteratorNext = CSVLine.IndexOf('\"');
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
                    int iteratorNext = CSVLine.IndexOf(',');
                    if (iteratorNext == -1)
                    {
                        throw new FormatException("Invalid Format of CSV Input");
                    }
                    string temp = CSVLine.Substring(iterator, iteratorNext - iterator);
                    Line[i] = temp;
                    iterator = iteratorNext + 1;
                }
            }
            return Line;
        }

        #endregion
    }
}
