using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moab.Models.Helpers;
using Xunit;
using FluentAssertions;

namespace MoabTests.Models.Helpers
{
    public class RTTLHelperTests
    {
        [Theory]
        [InlineData(30,65,'m',12)]
        [InlineData(31, 65, 'm', 12)]
        [InlineData(60, 65, 'M', 18)]
        [InlineData(20, 65, 'm', 8)]
        [InlineData(45, 79, 'm', 13)]
        [InlineData(30, 65, 'f', 11)]
        [InlineData(31, 65, 'f', 11)]
        [InlineData(60, 65, 'F', 16)]
        [InlineData(20, 65, 'f', 7)]
        [InlineData(45, 79, 'f', 12)]
        public void ShouldCalculateStandups(int duration, int age, char gender, int expectedResult)
        {
            //act
            var result = RTTLHelper.CalculateTargetStandupReps(duration, age, gender);
            //assert
            result.Should().Be(expectedResult);
        }
    }
}
