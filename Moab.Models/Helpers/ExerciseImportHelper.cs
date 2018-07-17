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
        #region Members

        #endregion

        #region Public Methods

        public bool Import(string importCSV, ICollection<Exercise> exercises)
        {
            // TODO: Write the implementation!!!!!
            //In my method UpdateExercise, I pass in a string array as the CSV file. This implies
            //that at some point we create a method that splits the lines of CSV into arrays. 
            //However, we can't do this using the split method because of the commas in some hints. 
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
        
        protected Exercise FindExtantExercsieInCollection(ICollection<Exercise> exercises, string exerciseCode)
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
            exercise.ExerciseCode = updateCSV[0];
            exercise.Name = updateCSV[1];
            exercise.EasierHint = updateCSV[6];
            exercise.HarderHint = updateCSV[7];
            exercise.HasRepetitionTarget = Convert.ToBoolean(updateCSV[5]); //Is UnitTarget has repetition target? Ask!!!
            exercise.DateLastUpdated = DateTime.Now;
        }
        #endregion
    }
}
