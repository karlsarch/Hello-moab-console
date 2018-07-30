using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nymbl.Models.POCO;
using Moab.Models.Helpers;
using Xunit;
using FluentAssertions;

namespace MoabTests.Models.Helpers
{
    public class ExerciseExportHelperShould
    {
        #region Constants
        #endregion


        #region Members

        public ICollection<Exercise> _inputExercises = new HashSet<Exercise>();

        #endregion


        #region Constructors

        /// <summary>
        ///     Constructor.  Populates the _inputExercises collection for use
        ///     by test methods
        /// </summary>
        /// <tag status=In-Progress/Compiles></tag>
        public ExerciseExportHelperShould()
        {
            var ex1 = new Exercise
            {
                ExerciseCode = "FLX_003_L",
                Name = "Old Calf stretch Left",
                EasierHint = "Easy, Partner",
                HarderHint = "As a diamond."
            };

            var ex2 = new Exercise
            {
                ExerciseCode = "FLX_003_R",
                Name = "Old Calf stretch Right",
                EasierHint = "Easy, Partner",
                HarderHint = "As a diamond."
            };

            var ex3 = new Exercise
            {
                Id = Guid.NewGuid(),
                ExerciseCode = "STAB_012_X",
                Name = "Standing weight shift",
                EasierHint = "Don't move",
                HarderHint = "Close your eyes"
            };

            var ex4 = new Exercise
            {
                ExerciseCode = "KSA_999_X",
                Name = "One Finger pull-up",
                EasierHint = "Use two fingers",
                HarderHint = "Pinky Finger"
            };

            _inputExercises.Add(ex1);
            _inputExercises.Add(ex2);
            _inputExercises.Add(ex3);
            _inputExercises.Add(ex4);
        }

        #endregion


        #region Tests

        [Fact]
        public void ConvertExercisesToCSV()
        {

        }

        #endregion
    }
}
