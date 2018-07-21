using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Moab.Models;

namespace MoabTests.Models
{
    public class Tour_TrailShoud
    {
        [Theory]
        [InlineData(0, 1000, 0, true)]
        [InlineData(0, 1000, 1, true)]
        [InlineData(0, 1000, 999, true)]
        [InlineData(0, 1000, 1000, true)]
        [InlineData(0, 1000, null, true)]
        [InlineData(10, 1000, 10, true)]
        [InlineData(10, 1000, 9, false)]
        [InlineData(10, 900, 901, false)]
        [InlineData(null, 1000, 9, true)]
        [InlineData(10, null, 1000, true)]
        [InlineData(null, null, null, true)]
        [InlineData(50, 50, 50, true)]
        [InlineData(50, 50, null, true)]
        [InlineData(null, 990, 1, true)]
        [InlineData(0, null, 1001, false)]
        public void Validate(int? minDifficulty, int? maxDifficulty, int? difficulty, bool isValidExpected)
        {
            // Arrange
            var trail = new Trail()
            {
                MinDifficulty = minDifficulty,
                MaxDifficulty = maxDifficulty
            };

            var objectUnderTest = new Tour_Trail
            {
                Trail = trail,
                Difficulty = difficulty
            };

            // Act
            var result = objectUnderTest.IsValid();

            // Assert
            result.Should().Be(isValidExpected);
        }
    }
}
