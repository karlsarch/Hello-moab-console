using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moab.Models.Helpers;

namespace MoabTests.Models.Helpers
{
    internal class CSVProcessorMock : ICSVProcessor
    {
        public string GenerateHeader(int numberOfHints)
        {
            return "ExerciseCode,Name,CDT_Class,CDT_AtHome,IsMovementDataCo" +
            "llected,UnitTarget,HintEasier,HintHarder,Hint1,Hint2,MDT_Class," +
            "MDT_AtHome,OldCode,Name_animationFile," +
            "Old_Name_animationFile";
        }
    }
}
