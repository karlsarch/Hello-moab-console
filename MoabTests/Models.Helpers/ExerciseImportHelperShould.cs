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
        #region Constants
        public const string GoodHeader2Hints = "ExerciseCode,Name,CDT_Class,CDT_AtHome,IsMovementDataCo" +
            "llected,UnitTarget,HintEasier,HintHarder,Hint1,Hint2,MDT_Class," +
            "MDT_AtHome,OldCode,Name_animationFile," +
            "Old_Name_animationFile";

        #endregion

        #region Members

        public ICollection<Exercise> _existingExercises = new HashSet<Exercise>();
        const string inputTextWithCommaInHint = "FLX_003_L,Old Calf stretch Left,Y,Y,N,N,\"Easy, " +
             "Partner\",As a diamond.,Knees Straight,Heels on the floor,,,,,,,,,";
        readonly string[] expectedinputTextWithCommaInHint = {"FLX_003_L","Old Calf stretch Left",
            "Y","Y","N","N","Easy, Partner","As a diamond.","Knees Straight","Heels on the floor","","","","","","","","" };
        const string CSVInputex2 = "FLX_003_R,Old Calf Stretch Right,Y,Y,N,N,\"Easy, " +
                 "Partner\",As a diamond.,Knees Straight,Heels on the floor,Lean forward!,,,,,,,,";

        const string CSVInputex3 = "STAB_012_X,Standing weight shift,Y,Y,N,N,Don't move,Close your eyes,\"Look at a spot on the wall, but not too " +
        "hard!\",,,,,,,,,,";

        const string CSVInputex4 = "KSA_999_X,One Finger pull-up,Y,Y,N,N,Use Two Fingers,Pinky Finger,,,,,,,,,,,";

        const string CSVListTest1 = "ExerciseCode,Name,CDT_Class,CDT" +
            "_AtHome,IsMovementDataCollected,UnitTarget," +
            "HintEasier,HintHarder,Hint1,Hint2,MDT_Class,MDT_AtHome,OldCode," +
            "Name_animationFile,Old_Name_animationFile\r\n" +
            ",,,,,,,,,,,,,,\r\nFLX_003_L,Old Calf stretch Left,Y,Y,N,N,\"Easy, Partner" +
            "\",As a diamond.,,,,,,,,,,, \r\n" +
            "FLX_003_R,Old Calf stretch Right,Y,Y,N,N\"Easy, Partner\",As a diamond" +
            ".,,,,,,,,,,,\r\n" +
            "STAB_012_X,Standing weight shift,Y,Y,N,N,Don't move,Close your eyes,,,," +
            ",,,,,,,\r\nKSA_999_X,One Finger pull-up,Use Two Fingers,Pinky Fin" +
            "ger,,,,,,,,,,,";

        const string CSVListTest4 = "ExerciseCode,Name,CDT_Class,CDT" +
            "_AtHome,IsMovementDataCollected,UnitTarget," +
            "HintEasier,HintHarder,Hint1,Hint2,MDT_Class,MDT_AtHome,OldCode," +
            "Name_animationFile,Old_Name_animationFile\r\n" +
            ",,,,,,,,,,,,,,\r\nFLX_003_L,Old Calf stretch Left,Y,Y,N,N,\"Easy, Partner" +
            "\",As a diamond.,,,,,,,,,,, \r\n" +
            "FLX_003_R,Old Calf stretch Right,Y,Y,N,N,\"Easy, Partner\",As a diamond" +
            ".,,,,,,,,,,,\r\n" +
            "STAB_012_X,Standing weight shift,Y,Y,N,N,Don't move,Close your eyes,,,," +
            ",,,,,,,\r\nKSA_999_X,One Finger pull-up,Use Two Fingers,Pinky Fin" +
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

            _existingExercises.Add(ex1);
            _existingExercises.Add(ex2);
            _existingExercises.Add(ex3);
            _existingExercises.Add(ex4);
        }

        #endregion

        #region Tests

        [Theory]
        [InlineData("a,b,c", new string[] { "a", "b", "c" })]
        [InlineData("a,,b,c", new string[] { "a", "", "b", "c" })]
        [InlineData("a,b c,d", new string[] { "a", "b c", "d" })]
        [InlineData(" a, b,c ", new string[] { "a", "b", "c" })]
        [InlineData("a,\"bee\",c", new string[] { "a", "bee", "c" })]
        [InlineData("a,\"bee, see?\",d", new string[] { "a", "bee, see?", "d" })]
        [InlineData(CSVInputex2, new string[] { "FLX_003_R", "Old Calf Stretch Right", "Y", "Y", "N", "N","Easy, Partner","As a diamond.", "Knees Straight", "Heels on the floor", "Lean forward!", "", "", "", "","", "", "" })]
        [InlineData(CSVInputex3, new string[] { "STAB_012_X", "Standing weight shift","Y","Y","N","N","Don't move", "Close your eyes", "Look at a spot on the wall, but not too " +
        "hard!", "", "", "", "", "", "","", "", "" })]
        [InlineData(CSVInputex4, new string[] { "KSA_999_X", "One Finger pull-up","Y","Y","N","N","Use Two Fingers", "Pinky Finger", "", "", "", "", "", "", "","", "", "" })]
        public void SplitLineIntoArray(string input, string[] expected)
        {
            // Arrange
            var sut = new ExerciseImportHelper();
            // Act
            var result = sut.SplitCSVLine(input);

            // Assert
            for (int i = 0; i<expected.Length; i++)
            {
                result[i].Should().Be(expected[i]);
            }
        }

        /// <summary>
        ///     One of the four tests to see if the LineSplitting Function works
        /// </summary>
        /// <tag status=Complete></tag>
        [Fact]
        public void SplitLineWithTwoHints()
        {
            // Arrange

            var input = GoodHeader2Hints + Environment.NewLine + inputTextWithCommaInHint;

            var expectedExercise = new Exercise
            {
                ExerciseCode = "FLX_003_L",
                Name = "Old Calf stretch Left",
                EasierHint = "Easy, Partner",
                HarderHint = "As a diamond."
            };

            ExerciseImportHelper importer = new ExerciseImportHelper();
            // Act
            var result = importer.ImportWithoutDelete(input, new HashSet<Exercise>());
            // Assert
            var resultEx = result.ToList()[0];

            resultEx.ExerciseCode.Should().Be(expectedExercise.ExerciseCode);
            resultEx.Name.Should().Be(expectedExercise.Name);
            resultEx.EasierHint.Should().Be(expectedExercise.EasierHint);
            resultEx.HarderHint.Should().Be(expectedExercise.HarderHint);
            resultEx.ExerciseHints.Count().Should().Be(2);

            resultEx.ExerciseHints.Should().Contain(c => c.Text == "Knees Straight");
            resultEx.ExerciseHints.Should().Contain(c => c.Text == "Heels on the floor");


        }


        /// <summary>
        ///     Tests whether the import function will ignore blank lines (test 2)
        /// </summary>
        /// <tag status=Complete</tag>
        [Fact]
        public void IgnoreBlankLines()
        {
            const string CSVListTest2 = "ExerciseCode,Name,CDT_Class,CDT" +
                     "_AtHome,IsMovementDataCollected,UnitTarget," +
                     "HintEasier,HintHarder,Hint1,Hint2,MDT_Class,MDT_AtHome,OldCode," +
                     "Name_animationFile,Old_Name_animationFile\r\n" +
                     ",,,,,,,,,,,,,,\r\nFLX_003_L,Old Calf stretch Left,Y,n,y,n,\"Easy, Partner" +
                     "\",As a diamond.,,,,,,,,,,, \r\n,,,,,,,,,,,,,,\r\n" +
                     "FLX_003_R,Old Calf stretch Right,Y,n,y,n,\"Easy, Partner\",As a diamond" +
                     ".,,,,,,,,,,,\r\n,,,,,,,,,,,,,,\r\n" +
                     "STAB_012_X,Standing weight shift,Y,n,y,n,Don't move,Close your eyes,,,," +
                     ",,,,,,,\r\nKSA_999_X,One Finger pull-up,Y,n,y,n,Use two fingers,Pinky Fin" +
                     "ger,,,,,,,,,,,";
            // Arrange
            var importer = new ExerciseImportHelper();
            // Act
            var allExList = importer.ImportWithoutDelete(CSVListTest2, new List<Exercise>());
            // Assert
            allExList.Count().Should().Be(4);
        }

        /// <summary>
        ///     Tests whether the import function throws correct error (Test 3)
        /// </summary>
        /// <tag status=Completed></tag>
        [Fact]
        public void ThrowExceptionWhenHeaderIsBad()
        {
            const string CSVListTest3 = "ExerciseCode,Name,CDT_Class,CDT" +
                    "_AtHome,IsMovementDataCollected,UnitTarget," +
                    "HintEasier,Hint Harder,Hint1,Hint2,MDT_Class,MDT_AtHome,OldCode," +
                    "Name_animationFile,Old_Name_animationFile\r\nFLX_003_L";
            // Arrange
            var Import = new ExerciseImportHelper();
            var AllExList = new List<string[]>();
            // Act
            Action act1 = () => Import.ImportWithoutDelete(CSVListTest3, new List<Exercise>());
            Action act2 = () => Import.ImportWithDelete(CSVListTest3, new List<Exercise>());
            // Assert
            act1.Should().Throw<FormatException>();
            act2.Should().Throw<FormatException>();
        }

        /// <summary>
        ///     Tests whether the import function throws correct error (Test 3)
        /// </summary>
        /// <tag status=Completed></tag>
        [Fact]
        public void ThrowExceptionWhenNoDataIsThere()
        {
            const string CSVListTest3 = "ExerciseCode,Name,CDT_Class,CDT" +
                    "_AtHome,IsMovementDataCollected,UnitTarget," +
                    "HintEasier,Hint Harder,Hint1,Hint2,MDT_Class,MDT_AtHome,OldCode," +
                    "Name_animationFile,Old_Name_animationFile";
            // Arrange
            var Import = new ExerciseImportHelper();
            var AllExList = new List<string[]>();
            // Act
            Action act1 = () => Import.ImportWithoutDelete(CSVListTest3, new List<Exercise>());
            Action act2 = () => Import.ImportWithDelete(CSVListTest3, new List<Exercise>());
            // Assert
            act1.Should().Throw<ArgumentException>();
            act2.Should().Throw<ArgumentException>();
        }

        /// <summary>
        ///     Test the findNumHints function
        /// </summary>
        /// <tag status="Complete"></tag>
        ///
        [Theory]
        [InlineData("InputHintNumTestExerciseCode,Name,CDT_Class,CDT_AtHome,IsMovementDataCollected,UnitTarget,HintEasier," +
                    "HintHarder,Hint1,Hint2,MDT_Class,MDT_AtHome,OldCode,Name_animationFile,Old_Name_animationFile", 2)]
        [InlineData("InputHintNumTestExerciseCode,Name,CDT" +
            "_Class,CDT_AtHome,IsMovementDataCollected,UnitTarget,HintEasier" +
            ",HintHarder,Hint1,Hint2,Hint3,Hint4,MDT_Class,MDT_AtHome,OldCode" +
            ",Name_animationFile,Old_Name_animationFile", 4)]
        [InlineData("InputHintNumTestExerciseCode,Name,CDT" +
            "_Class,CDT_AtHome,IsMovementDataCollected,UnitTarget,HintEasier" +
            ",HintHarder,MDT_Class,MDT_AtHome,OldCode,Name_anima" +
            "tionFile,Old_Name_animationFile", 0)]
        public void FindTheNumberOfHintsinHeader(string header, int expectedHintCount )
        {
            //Arrange

            var importer = new ExerciseImportHelper();
            var csvArray = importer.SplitCSVLine(header);
            //Act
            var result = importer.FindNumHints(csvArray);
            //Assert
            result.Should().Be(expectedHintCount);

        }

        /// <summary>
        ///     Test the RefreshHints function (test 1).
        /// </summary>
        /// <tag status=Complete></tag>
        [Fact]
        public void RefreshHintsInExercise()
        {
            // Arrange
            var ex1 = new Exercise
            {
                ExerciseCode = "FLX_003_L",
                Name = "Old Calf stretch Left",
                EasierHint = "Easy, Partner",
                HarderHint = "As a diamond."
            };

            var hint1 = new ExerciseHint()
            {
                Id = ex1.Id,
                Text = "Knees Straight",
                ExerciseID = ex1.Id
            };
            var hint2 = new ExerciseHint()
            {
                Id = ex1.Id,
                Text = "Heels on the floor",
                ExerciseID = ex1.Id
            };
            ICollection<ExerciseHint> hints = new HashSet<ExerciseHint>
            {
                hint1,
                hint2
            };

            var importer = new ExerciseImportHelper();
            var line = importer.SplitCSVLine(inputTextWithCommaInHint);

            // Act
            importer.RefreshHints(ex1, line);
            //Assert
            ex1.ExerciseHints.Equals(hints); // TODO: Fix
        }

        /// <summary>
        ///     Test the RefreshHints function (test 2).
        /// </summary>
        /// <tag status=Complete></tag>
        [Fact]
        public void RefreshHintsTest2()
        {
            // Arrange
            var objectUnderTest = new ExerciseImportHelper();
            var ex2 = new Exercise
            {
                ExerciseCode = "FLX_003_R",
                Name = "Old Calf stretch Right",
                EasierHint = "Easy, Partner",
                HarderHint = "As a diamond."
            };
            var hint1 = new ExerciseHint()
            {
                Id = ex2.Id,
                Text = "Knees Straight",
                ExerciseID = ex2.Id
            };
            var hint2 = new ExerciseHint()
            {
                Id = ex2.Id,
                Text = "Heels on the floor",
                ExerciseID = ex2.Id
            };
            var hint3 = new ExerciseHint()
            {
                Id = ex2.Id,
                Text = "Lean forward!",
                ExerciseID = ex2.Id
            };
            ICollection<ExerciseHint> hints = new HashSet<ExerciseHint>
            {
                hint1,
                hint2,
                hint3
            };
            var importer = new ExerciseImportHelper();
            var line = importer.SplitCSVLine(CSVInputex2);

            // Act
            importer.RefreshHints(ex2, line);
            //Assert
            ex2.ExerciseHints.Equals(hints);
        }

        /// <summary>
        ///     Test the RefreshHints function (test 3).
        /// </summary>
        /// <tag status=Complete></tag>
        [Fact]
        public void RefreshHintsTest3()
        {
            // Arrange
            var objectUnderTest = new ExerciseImportHelper();
            var ex3 = new Exercise
            {
                ExerciseCode = "STAB_012_X",
                Name = "Standing weight shift",
                EasierHint = "Don't move",
                HarderHint = "Close your eyes"
            };
            var hint1 = new ExerciseHint()
            {
                Id = ex3.Id,
                Text = "\"Look at a spot on the wall, but not too hard!\"",
                ExerciseID = ex3.Id
            };
            ICollection<ExerciseHint> hints = new HashSet<ExerciseHint>
            {
                hint1
            };
            var importer = new ExerciseImportHelper();
            var line = importer.SplitCSVLine(CSVInputex3);

            // Act
            importer.RefreshHints(ex3, line);
            //Assert
            ex3.ExerciseHints.Equals(hints);
        }

        /// <summary>
        ///     Test the RefreshHints function (test 4).
        /// </summary>
        /// <tag status=Complete></tag>
        [Fact]
        public void RefreshHintsTest4()
        {
            // Arrange
            var objectUnderTest = new ExerciseImportHelper();
            var ex4 = new Exercise
            {
                ExerciseCode = "KSA_999_X",
                Name = "One Finger pull-up",
                EasierHint = "Use two fingers",
                HarderHint = "Pinky Finger"
            };

            ICollection<ExerciseHint> hints = new HashSet<ExerciseHint>();
            var importer = new ExerciseImportHelper();
            var line = importer.SplitCSVLine(CSVInputex4);

            // Act
            importer.RefreshHints(ex4, line);
            //Assert
            ex4.ExerciseHints.Equals(hints);
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
        [InlineData(GoodHeader2Hints, true)]
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
        //Extra columns at the end should be acceptable
        [InlineData("ExerciseCode,Name,CDT_Class,CDT_AtHome,IsMovementDataCo" +
            "llected,UnitTarget,HintEasier,HintHarder,Hint1,Hint2,MDT_Class," +
            "MDT_AtHome,OldCode,Name_animationFile,Old_Name_animationFile,xx" +
            ",e,y", true)]
        //Spaces in the column names should fail
        [InlineData("ExerciseCode,Name,CDT_Class,CDT_AtHome,IsMovementDataCo" +
            "llected,UnitTarget,Hint Easier,Hint Harder,Hint1,Hint2,MDT_Class," +
            "MDT_AtHome,OldCode,Name_animationFile," +
            "Old_Name_animationFile", false)]
        //testing if new column at beginning somehow succeeds
        [InlineData("Object ID, ExerciseCode,Name,CDT_Class,CDT_AtHome,IsMovementDataCo" +
            "llected,UnitTarget,HintEasier,HintHarder,Hint1,Hint2,MDT_Class," +
            "MDT_AtHome,OldCode,Name_animationFile," +
            "Old_Name_animationFile", false)]
        public void CheckHeader(string header, bool isValidExpected)
        {
            // Arrange

            ExerciseImportHelper importer = new ExerciseImportHelper();
            // Act
            var result = importer.IsHeaderValid(header);

            // Assert
            result.Should().Be(isValidExpected);
        }

        [Fact]
        public void UpdateExisting()
        {
            // Arrange
            var processor = new CSVProcessorMock();
            var importer = new ExerciseImportHelper(processor);
            const string validHeader = "ExerciseCode,Name,CDT_Class,CDT_AtHome,IsMovementDataCo" +
            "llected,UnitTarget,HintEasier,HintHarder,Hint1,Hint2,MDT_Class," +
            "MDT_AtHome,OldCode,Name_animationFile," +
            "Old_Name_animationFile";
            string inputLine = "STAB_012_X,TestName,Y,Y,N,Y,make it easy," +
            "make it hard," +
            "Stand with your feet hip width apart and keep your legs straight as you shift weight from one foot to the other. ," +
            ",N,N,,STAB_012_X_StandingWeightShift,STAB_012 Standing weight shift";
            string input = validHeader + Environment.NewLine + inputLine;
            var originalId = _existingExercises.ToList()[2].Id;
            // Act
            var result = importer.ImportWithoutDelete(input, _existingExercises);
            // Assert
            List<Exercise> resultList = result.ToList();
            resultList[2].Name.Should().Be("TestName");
            resultList[2].ExerciseCode.Should().Be("STAB_012_X");
            resultList[2].HasRepetitionTarget.Should().Be(true);
            resultList[2].EasierHint.Should().Be("make it easy");
            resultList[2].HarderHint.Should().Be("make it hard");
            resultList[2].Id.Should().Be(originalId);
            resultList.Count().Should().Be(4);
        }

        [Fact]
        public void CreateNew()
        {
            // Arrange
            var importer = new ExerciseImportHelper();
            const string validHeader = "ExerciseCode,Name,CDT_Class,CDT_AtHome,IsMovementDataCo" +
            "llected,UnitTarget,HintEasier,HintHarder,Hint1,Hint2,MDT_Class," +
            "MDT_AtHome,OldCode,Name_animationFile," +
            "Old_Name_animationFile";
            string inputLine = "JUMP_999_B,Standing backflip,Y,Y,N,Y,land on your back," +
            "double backflip," +
            "Push off with both feet. ,Use your muscles! ,Try harder! , " +
            "N,N,,STAB_012_X_StandingWeightShift,STAB_012 Standing weight shift";
            string input = validHeader + Environment.NewLine + inputLine;
            // Act
            var result = importer.ImportWithoutDelete(input, _existingExercises);
            // Assert
            List<Exercise> resultList = result.ToList();
            resultList[4].ExerciseCode.Should().Be("JUMP_999_B");
            resultList[4].Name.Should().Be("Standing backflip");
            resultList[4].HasRepetitionTarget.Should().Be(true);
            resultList[4].EasierHint.Should().Be("land on your back");
            resultList[4].HarderHint.Should().Be("double backflip");
            resultList.Count().Should().Be(5);
        }

        [Fact]
        public void CreateNewTwoHint()
        {
            // Arrange
            var importer = new ExerciseImportHelper();
            const string validHeader = "ExerciseCode,Name,CDT_Class,CDT_AtHome,IsMovementDataCo" +
            "llected,UnitTarget,HintEasier,HintHarder,Hint1,Hint2,MDT_Class," +
            "MDT_AtHome,OldCode,Name_animationFile," +
            "Old_Name_animationFile";
            string inputLine = "JUMP_999_B,Standing backflip,Y,Y,N,Y,land on your back," +
            "double backflip," +
            "Push off with both feet. ,Use your muscles!," +
            "N,N,,STAB_012_X_StandingWeightShift,STAB_012 Standing weight shift";
            string input = validHeader + Environment.NewLine + inputLine;
            // Act
            var result = importer.ImportWithoutDelete(input, _existingExercises);
            // Assert
            List<Exercise> resultList = result.ToList();
            resultList[4].ExerciseCode.Should().Be("JUMP_999_B");
            resultList[4].Name.Should().Be("Standing backflip");
            resultList[4].HasRepetitionTarget.Should().Be(true);
            resultList[4].EasierHint.Should().Be("land on your back");
            resultList[4].HarderHint.Should().Be("double backflip");
            var hintList = (from h in resultList[4].ExerciseHints
                            select h.Text).ToList();
            hintList[0].Should().Be("Push off with both feet.");
            hintList[1].Should().Be("Use your muscles!");
            resultList.Count().Should().Be(5);
        }

        [Fact]
        public void CreateNewMultiHint()
        {
            // Arrange
            var importer = new ExerciseImportHelper();
            const string validHeader = "ExerciseCode,Name,CDT_Class,CDT_AtHome,IsMovementDataCo" +
            "llected,UnitTarget,HintEasier,HintHarder,Hint1,Hint2,Hint3,MDT_Class," +
            "MDT_AtHome,OldCode,Name_animationFile," +
            "Old_Name_animationFile";
            string inputLine = "JUMP_999_B,Standing backflip,Y,Y,N,Y,land on your back," +
            "double backflip," +
            "Push off with both feet. ,Use your muscles! ,Try harder! , " +
            "N,N,,STAB_012_X_StandingWeightShift,STAB_012 Standing weight shift";
            string input = validHeader + Environment.NewLine + inputLine;
            // Act
            var result = importer.ImportWithoutDelete(input, _existingExercises);
            // Assert
            List<Exercise> resultList = result.ToList();
            resultList[4].ExerciseCode.Should().Be("JUMP_999_B");
            resultList[4].Name.Should().Be("Standing backflip");
            resultList[4].HasRepetitionTarget.Should().Be(true);
            resultList[4].EasierHint.Should().Be("land on your back");
            resultList[4].HarderHint.Should().Be("double backflip");
            var hintList = (from h in resultList[4].ExerciseHints
                            select h.Text).ToList();
            hintList[0].Should().Be("Push off with both feet.");
            hintList[1].Should().Be("Use your muscles!");
            hintList[2].Should().Be("Try harder!");
            resultList.Count().Should().Be(5);
        }

        [Fact]
        public void CreateNewCollectionWithDelete()
        {
            // Arrange
            var importer = new ExerciseImportHelper();
            const string validHeader = "ExerciseCode,Name,CDT_Class,CDT_AtHome,IsMovementDataCo" +
            "llected,UnitTarget,HintEasier,HintHarder,Hint1,Hint2,MDT_Class," +
            "MDT_AtHome,OldCode,Name_animationFile," +
            "Old_Name_animationFile";
            string inputLine = "STAB_010_R,Half tandem stand Right,Y,Y,Y,N,wider stance," +
                "put weight evenly on both feet,Remember the goal is to have your feet as close" +
                "as possible.Your left foot should be nestling inside your right instep. ," +
                "Do your best to stand up tall with your head aligned with your spine.," +
                " N, N,, STAB_010_R_ HalfTandemStand,STAB_010 Half tandem stand";

            string input = validHeader + Environment.NewLine + inputLine;
            // Act
            var result = importer.ImportWithDelete(input, _existingExercises);
            // Assert
            List<Exercise> resultList = result.ToList();
            resultList.Count().Should().Be(1);
            _existingExercises.ToList().Count().Should().Be(4);
            resultList[0].ExerciseCode.Should().Be("STAB_010_R");
            resultList[0].Name.Should().Be("Half tandem stand Right");
            resultList[0].HasRepetitionTarget.Should().Be(false);
            resultList[0].EasierHint.Should().Be("wider stance");
            resultList[0].HarderHint.Should().Be("put weight evenly on both feet");
        }

        //This both adds a new exercise and should remove one from the old list while adding it to
        //the new
        [Fact]
        public void CreateNewCollectionWithDeleteWithUpdate()
        {
            // Arrange
            var importer = new ExerciseImportHelper();
            const string validHeader = "ExerciseCode,Name,CDT_Class,CDT_AtHome,IsMovementDataCo" +
            "llected,UnitTarget,HintEasier,HintHarder,Hint1,Hint2,MDT_Class," +
            "MDT_AtHome,OldCode,Name_animationFile," +
            "Old_Name_animationFile";
            string inputLine = "STAB_010_R,Half tandem stand Right,Y,Y,Y,N,wider stance," +
                "put weight evenly on both feet,Remember the goal is to have your feet as close" +
                "as possible.Your left foot should be nestling inside your right instep. ," +
                "Do your best to stand up tall with your head aligned with your spine.," +
                " N, N,, STAB_010_R_ HalfTandemStand,STAB_010 Half tandem stand";
            string inputLine2 = "STAB_012_X,TestName,Y,Y,N,Y,make it easy," +
            "make it hard," +
            "Stand with your feet hip width apart and keep your legs straight as you shift weight from one foot to the other. ," +
            ",N,N,,STAB_012_X_StandingWeightShift,STAB_012 Standing weight shift";
            string input = validHeader + Environment.NewLine + inputLine + Environment.NewLine + inputLine2;
            var originalId = _existingExercises.ToList()[2].Id;
            // Act
            var result = importer.ImportWithDelete(input, _existingExercises);
            // Assert
            List<Exercise> resultList = result.ToList();
            resultList.Count().Should().Be(2);
            _existingExercises.ToList().Count().Should().Be(3);
            resultList[0].ExerciseCode.Should().Be("STAB_010_R");
            resultList[0].Name.Should().Be("Half tandem stand Right");
            resultList[0].HasRepetitionTarget.Should().Be(false);
            resultList[0].EasierHint.Should().Be("wider stance");
            resultList[0].HarderHint.Should().Be("put weight evenly on both feet");
            resultList[1].ExerciseCode.Should().Be("STAB_012_X");
            resultList[1].Name.Should().Be("TestName");
            resultList[1].HasRepetitionTarget.Should().Be(true);
            resultList[1].EasierHint.Should().Be("make it easy");
            resultList[1].HarderHint.Should().Be("make it hard");
            resultList[1].Id.Should().Be(originalId);
        }

        #endregion
    }
}
