using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moab.Models.Helpers
{
    public enum ExerciseCSVPreHintColumns
    {
        ExerciseCode, Name, CDT_Class, CDT_AtHome,
        IsMovementDataCollected, UnitTarget, HintEasier, HintHarder
    }
    public enum ExerciseCSVPostHintColumns
    {
        MDT_Class, MDT_AtHome, OldCode, Name_animationFile,
        Old_Name_animationFile
    }
    internal class CSVProcessor : ICSVProcessor
    {
        public string GenerateHeader(int numberOfHints)
        {
            var preValues = Enum.GetNames(typeof(ExerciseCSVPreHintColumns));
            var postValues = Enum.GetNames(typeof(ExerciseCSVPostHintColumns));
            var values = new List<string>();
            values.AddRange(preValues);
            for (int j = 0; j < numberOfHints; j++)
            {
                values.Add($"Hint{j + 1}");
            }
            values.AddRange(postValues);

            var sb = new StringBuilder($"{values[0]}");
            for (int i = 1; i < values.Count; i++)
            {
                sb.Append($",{values[i]}");
            }

            return sb.ToString();
        }
    }
}
