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
        string[] CSVLineex1 = { "FLX_003_L", "Old Calf Stretch Left",
            "Easy, Partner", "As a diamond", "", "", "", "", "", "", "",
            "", "", "" };
        const string CSVInputex2 = "FLX_003_R,Old Calf stretch Right,\"Easy," +
            " Partner\",As a diamond.,,,,,,,,,,,";
        string[] CSVLineex2 = { "FLX_003_R", "Old Calf Stretch Right",
            "Easy, Partner", "As a diamond", "", "", "", "", "", "", "",
            "", "", "" };
        const string CSVInputex3 = "STAB_012_X,Standing weight shift,Don't move," +
            "Close your eyes,,,,,,,,,,,";
        string[] CSVLineex3 = { "STAB_012_X", "Standing Weight Shift",
            "Don't Move", "Close your eyes", "", "", "", "", "", "", "",
            "", "", "" };
        const string CSVInputex4 = "KSA_999_X,One Finger pull-up,Use two fin" +
            "gers,Pinky Finger,,,,,,,,,,,";
        string[] CSVLineex4 = { "KSA_999_X", "One Finger pull-up",
            "Use Two Fingers", "Pinky Finger", "", "", "", "", "", "", "",
            "", "", "" };
        const string CSVListTest1 = "ExerciseCode,Name,CDT_Class,CDT" +
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
        const string CSVListTest2 = "ExerciseCode,Name,CDT_Class,CDT" +
            "_AtHome,IsMovementDataCollected,UnitTarget," +
            "HintEasier,HintHarder,Hint1,Hint2,MDT_Class,MDT_AtHome,OldCode," +
            "Name_animationFile,Old_Name_animationFile\n" +
            ",,,,,,,,,,,,,,\nFLX_003_L,Old Calf stretch Left,\"Easy, Partner" +
            "\",As a diamond.,,,,,,,,,,, \n,,,,,,,,,,,,,,\n" +
            "FLX_003_R,Old Calf stretch Right,\"Easy, Partner\",As a diamond" +
            ".,,,,,,,,,,,\n,,,,,,,,,,,,,,\n" +
            "STAB_012_X,Standing weight shift,Don't move,Close your eyes,,,," +
            ",,,,,,,\nKSA_999_X,One Finger pull-up,Use two fingers,Pinky Fin" +
            "ger,,,,,,,,,,,";
        const string CSVListTest3 = "ExerciseCode,Name,CDT_Class,CDT" +
            "_AtHome,IsMovementDataCollected,UnitTarget," +
            "HintEasier,HintHarder,Hint1,Hint2,MDT_Class,MDT_AtHome,OldCode," +
            "Name_animationFile,Old_Name_animationFile";
        const string CSVListTest4 = "ExerciseCode,Name,CDT_Class,CDT" +
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
            int count = _existingExercises.Count;
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
            string[] ex1Split = new string[14];
            // Act
            ex1Split = TestImport.TestLineSplit(CSVInputex1);
            // Assert
            ex1Split.Should().Equals(CSVLineex1);
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
            string[] ex2Split = new string[14];
            // Act
            ex2Split = TestImport.TestLineSplit(CSVInputex2);
            // Assert
            ex2Split.Should().Equals(CSVLineex2);
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
            string[] ex3Split = new string[14];
            // Act
            ex3Split = TestImport.TestLineSplit(CSVInputex3);
            // Assert
            ex3Split.Should().Equals(CSVLineex3);
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
            string[] ex4Split = new string[14];
            // Act
            ex4Split = TestImport.TestLineSplit(CSVInputex4);
            // Assert
            ex4Split.Should().Equals(CSVLineex4);
        }

        /// <summary>
        ///     Tests whether the import function works (Test 1)
        /// </summary>
        /// <tag status=Complete></tag>
        [Fact]
        public void CSVImporttoListTesting1()
        {
            // Arrange
            var Import = new ExerciseImportHelper();
            var AllExList = new List<string[]>();
            var ResultList = new List<string[]>();
            ResultList.Add(CSVLineex1);
            ResultList.Add(CSVLineex2);
            ResultList.Add(CSVLineex3);
            ResultList.Add(CSVLineex4);
            // Act
            AllExList = Import.TestInputProcessing(CSVListTest1);
            // Assert
            AllExList.Should().Equals(ResultList);
        }

        /// <summary>
        ///     Tests whether the import function will ignore blank lines (test 2)
        /// </summary>
        /// <tag status=Complete</tag>
        [Fact]
        public void CSVImporttoListTesting2()
        {
            // Arrange
            var Import = new ExerciseImportHelper();
            var AllExList = new List<string[]>();
            var ResultList = new List<string[]>();
            ResultList.Add(CSVLineex1);
            ResultList.Add(CSVLineex2);
            ResultList.Add(CSVLineex3);
            ResultList.Add(CSVLineex4);
            // Act
            AllExList = Import.TestInputProcessing(CSVListTest2);
            // Assert
            AllExList.Should().Equals(ResultList);
        }

        /// <summary>
        ///     Tests whether the import function throws correct error (Test 3)
        /// </summary>
        /// <tag status=Completed></tag>
        [Fact]
        public void CSVImporttoListTesting3()
        {
            // Arrange
            var Import = new ExerciseImportHelper();
            var AllExList = new List<string[]>();
            // Act
            Action act = () => Import.TestInputProcessing(CSVListTest3);
            // Assert
            act.Should().Throw<FormatException>();
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
        //perfectly formatted header should work
        [InlineData("ExerciseCode,Name,CDT_Class,CDT_AtHome,IsMovementDataCo" +
            "llected,UnitTarget,HintEasier,HintHarder,Hint1,Hint2,MDT_Class," +
            "MDT_AtHome,OldCode,Name_animationFile," +
            "Old_Name_animationFile", true)]
        //missing column at end should not work
        [InlineData("ExerciseCode,Name,CDT_Class,CDT_AtHome,IsMovementDataCo" +
            "llected,UnitTarget,HintEasier,HintHarder,Hint1,Hint2,MDT_Class," +
            "MDT_AtHome,OldCode,Name_animationFile,", false)]
        //missing column in middle should not work
        [InlineData("ExerciseCode,Name,CDT_Class,CDT_AtHome,IsMovementDataCo" +
            "llected,UnitTarget,HintEasier,HintHarder,Hint1,Hint2,MDT_Class," +
            "Old_Name_animationFile", false)]
        [InlineData("ExerciseCode,Name,CDT_Class,IsMovementDataCollected,CDT" +
            "_AtHome,UnitTarget,CDT_AtHome,HintEasier,HintHarder,Hint1,Hint2" +
            ",MDT_Class,MDT_AtHome,OldCode,Name_animationFile,Old_Name_anima" +
            "tionFile", false)]
        [InlineData("ExerciseCode,Name,CDT_AtHome,CDT_Class,IsMovementDataCo" +
            "llected,UnitTarget,CDT_AtHome,HintEasier,HintHarder,Hint1,Hint2" +
            ",MDT_Class,MDT_AtHome,OldCode,Name_animationFile,Old_Name_anima" +
            "tionFile", false)]
        //testing if case affects result
        [InlineData("ExerciseCode,name,CDT_Class,CDT_AtHome,IsMovementDataCo" +
            "llected,UnitTarget,HintEasier,HintHarder,Hint1,Hint2,MDT_Class," +
            "MDT_AtHome,OldCode,Name_animationFile," +
            "Old_Name_animationFile", true)]
        //testing if extra columns at the end affect result
        [InlineData("ExerciseCode,Name,CDT_Class,CDT_AtHome,IsMovementDataCo" +
            "llected,UnitTarget,HintEasier,HintHarder,Hint1,Hint2,MDT_Class," +
            "MDT_AtHome,OldCode,Name_animationFile,Old_Name_animationFile,xx" +
            ",e,y", true)]
        //tests if spaces in the column names affect result
        [InlineData("ExerciseCode,Name,CDT_Class,CDT_AtHome,IsMovementDataCo" +
            "llected,UnitTarget,Hint Easier,Hint Harder,Hint1,Hint2,MDT_Class," +
            "MDT_AtHome,OldCode,Name_animationFile," +
            "Old_Name_animationFile", true)]
        //testing if new column at beginning somehow succeeds
        [InlineData("Object ID, ExerciseCode,Name,CDT_Class,CDT_AtHome,IsMovementDataCo" +
            "llected,UnitTarget,HintEasier,HintHarder,Hint1,Hint2,MDT_Class," +
            "MDT_AtHome,OldCode,Name_animationFile," +
            "Old_Name_animationFile", false)]

        public void CheckHeaderTest(string header, bool isValidExpected)
        {
            // Arrange

            ExerciseImportHelper HeaderTest = new ExerciseImportHelper();
            // Act
            var result = HeaderTest.TestIsHeaderValid(header);

            // Assert
            result.Should().Be(isValidExpected);
        }

        [Fact]
        public void UpdateExisting()
        {
            // Arrange
            var importer = new ExerciseImportHelper();
            const string validHeader = "ExerciseCode,Name,CDT_Class,CDT_AtHome,IsMovementDataCo" +
            "llected,UnitTarget,HintEasier,HintHarder,Hint1,Hint2,MDT_Class," +
            "MDT_AtHome,OldCode,Name_animationFile," +
            "Old_Name_animationFile";
            string inputLine = "STAB_012_X,Standing weight shift,Y,Y,N,N,keep feet on floor," + 
            "lift non-weight bearing foot off floor," + 
            "Stand with your feet hip width apart and keep your legs straight as you shift weight from one foot to the other. ," + 
            ",N,N,,STAB_012_X_StandingWeightShift,STAB_012 Standing Weight Shift";
            string input = validHeader + Environment.NewLine + inputLine;
            // Act
            var result = importer.Import(input, _existingExercises);
            // Assert
            List<Exercise> resultList = result.ToList();
            resultList[2].Name.Should().Be("TestName");
            resultList.Count().Should().Be(4);
        }

        #endregion
    }
}
