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
    /// <summary>
    ///     Tests the ExerciseImportHelper Class
    /// </summary>
    public class ExerciseImportHelperShould
    {
        #region Members

        public ICollection<Exercise> _existingExercises = new HashSet<Exercise>();
        const string CSVInputex1 = "FLX_003_L,Old Calf stretch Left,\"Easy, " +
            "Partner\",As a diamond.,,,,,,,,,,,";
        const string CSVInputex2 = "FLX_003_R,Old Calf stretch Right,\"Easy," +
            " Partner\",As a diamond.,,,,,,,,,,,";
        const string CSVInputex3 = "STAB_012_X,Standing weight shift,Don't m" +
            "ove,Close your eyes,,,,,,,,,,,";
        const string CSVInputex4 = "KSA_999_X,One Finger pull-up,Use two fin" +
            "gers,Pinky Finger,,,,,,,,,,,";
        const string CSVInputAllExercises = "ExerciseCode,Name,CDT_Class,CDT" +
            "_AtHome,IsMovementDataCollected,UnitTarget," +
            "HintEasier,HintHarder,Hint1,Hint2,MDT_Class,MDT_AtHome,OldCode," +
            "Name_animationFile,Old_Name_animationFile\n" +
            ",,,,,,,,,,,,,,\nFLX_003_L,Old Calf stretch Left,\"Easy, Partner" +
            "\",As a diamond.,,,,,,,,,,, \n" +
            "FLX_003_R,Old Calf stretch Right,\"Easy, Partner\",As a diamond" +
            ".,,,,,,,,,,,\n" +
            "STAB_012_X,Standing weight shift,Don't move,Close your eyes,,,," +
            ",,,,,,,\nKSA_999_X,One Finger pull-up,Use two fingers,Pinky Fin" +
            "ger,,,,,,,,,,,";

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructor.  Populates the existingExercise collection for use
        ///     by test methods
        /// </summary>
        /// <tag status=In-Progress/Compiles></tag>
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

        #endregion

        #region Tests

        /// <summary>
        ///     Tests whether there are four exercises
        /// </summary>
        /// <tag status=Complete></tag>
        [Fact]
        public void HaveFourExercises()
        {
            // Arrange
            // Act
            var count = _existingExercises.Count;
            // Assert

            count.Should().Be(4);
        }

        /// <summary>
        ///     One of the four tests to see if the LineSplitting Function works
        /// </summary>
        /// <tag status=Complete></tag>
        [Fact]
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
            // Assert
            CSVInputex1.Should().Equals(ex1test);
        }

        /// <summary>
        ///     Another function to test if the Line Splitting function works
        /// </summary>
        /// <tag status=Complete></tag>
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

        /// <summary>
        ///     A third function to test if the line splitting function works
        /// </summary>
        /// <tag status=Complete></tag>
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

        /// <summary>
        ///     The fourth function to test is the line splitting function works
        /// </summary>
        /// <tag status=Complete></tag>
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

        /// <summary>
        ///     Tests whether the entire import function works
        /// </summary>
        /// <tag status=Complete></tag>
        [Fact]
        public void CSVImporttoListTesting()
        {
            // Arrange
            ExerciseImportHelper Import = new ExerciseImportHelper();
            // Act
            List<string[]> AllExList = new List<string[]>();
            AllExList = Import.TestInputProcessing(CSVInputAllExercises);
            string AllExtest = "ExerciseCode,Name,CDT_Class,CDT_AtHome,IsMovementDataCollected,UnitTarget," +
            "HintEasier,HintHarder,Hint1,Hint2,MDT_Class,MDT_AtHome,OldCode,Name_animationFile,Old_Name_animationFile\n" +
            ",,,,,,,,,,,,,,\n";
            for (int i = 1; i < AllExList.Count; i++)
            {
                for (int j = 1; j < 14; j++)
                {
                    AllExtest += (AllExList[i][j] + ',');
                }
                AllExtest += '\n';
            }
            // Assert
            AllExtest.Should().Equals(CSVInputAllExercises);
        }

        /// <summary>
        /// Tests whether inner checkheader function works
        ///     <paramref name="header">
        ///         The possible header passed in
        ///     </paramref>
        ///     <paramref name="isValidExpected">
        ///         The value expected to be returned
        ///     </paramref>
        ///     <tag status=Complete></tag>
        /// </summary>
        [Theory]
        [InlineData("ExerciseCode,Name,CDT_Class,CDT_AtHome,IsMovementDataCollected,UnitTarget,HintEasier,HintHarder,Hint1,Hint2,MDT_Class,MDT_AtHome,OldCode,Name_animationFile,Old_Name_animationFile", true)]
        [InlineData("ExerciseCode,Name,CDT_Class,CDT_AtHome,IsMovementDataCollected,UnitTarget,HintEasier,HintHarder,Hint1,Hint2,MDT_Class,MDT_AtHome,OldCode,Name_animationFile,", false)]
        [InlineData("ExerciseCode,Name,CDT_Class,CDT_AtHome,IsMovementDataCollected,UnitTarget,HintEasier,HintHarder,Hint1,Hint2,MDT_Class,Old_Name_animationFile", false)]
        [InlineData("ExerciseCode,Name,CDT_Class,IsMovementDataCollected,CDT_AtHome,UnitTarget,CDT_AtHome,HintEasier,HintHarder,Hint1,Hint2,MDT_Class,MDT_AtHome,OldCode,Name_animationFile,Old_Name_animationFile", false)]
        [InlineData("ExerciseCode,Name,CDT_AtHome,CDT_Class,IsMovementDataCollected,UnitTarget,CDT_AtHome,HintEasier,HintHarder,Hint1,Hint2,MDT_Class,MDT_AtHome,OldCode,Name_animationFile,Old_Name_animationFile", false)]
        [InlineData("ExerciseCode,name,CDT_Class,CDT_AtHome,IsMovementDataCollected,UnitTarget,HintEasier,HintHarder,Hint1,Hint2,MDT_Class,MDT_AtHome,OldCode,Name_animationFile,Old_Name_animationFile", true)]
        [InlineData("ExerciseCode,Name,CDT_Class,CDT_AtHome,IsMovementDataCollected,UnitTarget,HintEasier,HintHarder,Hint1,Hint2,MDT_Class,MDT_AtHome,OldCode,Name_animationFile,Old_Name_animationFile,xx,e,y", true)]

        public void CheckHeaderTest(string header, bool isValidExpected)
        {
            // Arrange

            ExerciseImportHelper HeaderTest = new ExerciseImportHelper();
            // Act
            var result = HeaderTest.TestIsHeaderValid(header);

            // Assert
            result.Should().Be(isValidExpected);
        }

        #endregion
    }
}
