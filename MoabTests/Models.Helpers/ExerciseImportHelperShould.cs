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
    public class ExerciseImportHelperShould
    {
        public ICollection<Exercise> _existingExercises = new HashSet<Exercise>();
        string CSVInputex1 = "FLX_003_L,Old Calf stretch Left,\"Easy, Partner\",As a diamond.,,,,,,,,,,,";
        string CSVInputex2 = "FLX_003_R,Old Calf stretch Right,\"Easy, Partner\",As a diamond.,,,,,,,,,,,";
        string CSVInputex3 = "STAB_012_X,Standing weight shift,Don't move,Close your eyes,,,,,,,,,,,";
        string CSVInputex4 = "KSA_999_X,One Finger pull-up,Use two fingers,Pinky Finger,,,,,,,,,,,";
        string CSVInputAllExercises = "FLX_003_L,Old Calf stretch Left,\"Easy, Partner\",As a diamond.,,,,,,,,,,, \r\n" +
            "FLX_003_R,Old Calf stretch Right,\"Easy, Partner\",As a diamond.,,,,,,,,,,,\r\n" +
            "STAB_012_X,Standing weight shift,Don't move,Close your eyes,,,,,,,,,,," +
            "KSA_999_X,One Finger pull-up,Use two fingers,Pinky Finger,,,,,,,,,,,";

        /// <summary>
        /// Constructor.  Populates the existingExercise collection for use by test methods
        /// </summary>
        public ExerciseImportHelperShould()
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

            _existingExercises.Add(ex1);
            _existingExercises.Add(ex2);
            _existingExercises.Add(ex3);
            _existingExercises.Add(ex4);
        }

        // Add Unit Tests here...
        [Fact]
        public void HaveFourExercises()
        {
            // Arrange
            // Act
            var count = _existingExercises.Count;
            // Assert

            count.Should().Be(4);
        }

        [Fact]
        // Checks whether the LineSplitting Function works
        public void LineSplitting1()
        {
            // Arrange
            ExerciseImportHelper TestImport = new ExerciseImportHelper();
            // Act
            string[] ex1Split = new string[14];
            ex1Split = TestImport.TestLineSplit(CSVInputex1);
            string ex1test = ex1Split[0] + ',';
            for (int i = 1; i < 14; i++)
            {
                ex1test += (ex1Split[i] + ',');
            }
            string[] ex2Split = new string[14];
            ex1Split = TestImport.TestLineSplit(CSVInputex2);
            // Assert
            CSVInputex1.Should().Equals(ex1test);
        }

        [Fact]
        public void TestSplitting2()
        {
            // Arrange
            ExerciseImportHelper TestImport = new ExerciseImportHelper();
            // Act
            string[] ex2Split = new string[14];
            string ex2test = ex2Split[0] + ',';
            for (int i = 1; i < 14; i++)
            {
                ex2test += (ex2Split[i] + ',');
            }
            // Assert
            CSVInputex2.Should().Equals(ex2test);
        }

        [Fact]
        public void TestSplitting3()
        {
            // Arrange
            ExerciseImportHelper TestImport = new ExerciseImportHelper();
            // Act
            string[] ex3Split = new string[14];
            string ex3test = ex3Split[0] + ',';
            for (int i = 1; i < 14; i++)
            {
                ex3test += (ex3Split[i] + ',');
            }
            // Assert
            CSVInputex3.Should().Equals(ex3test);
        }

        [Fact]
        public void TestSplitting4()
        {
            // Arrange
            ExerciseImportHelper TestImport = new ExerciseImportHelper();
            // Act
            string[] ex4Split = new string[14];
            string ex4test = ex4Split[0] + ',';
            for (int i = 1; i < 14; i++)
            {
                ex4test += (ex4Split[i] + ',');
            }
            // Assert
            CSVInputex4.Should().Equals(ex4test);
        }
    }
}
