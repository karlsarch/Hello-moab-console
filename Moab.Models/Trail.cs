using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Moab.Models
{
    public class Trail
    {
        public string Name { get; set; }

        [Range(0,1000)]
        public int? MinDifficulty { get; set; }

        [Range(0,1000)]
        public int? MaxDifficulty { get; set; }

        /// <summary>
        /// Business logic to make sure we have a valid configuration; ensures MaxDifficulty >= MinDifficulty.
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            if ((MinDifficulty ?? 0) <= (MaxDifficulty ?? MinDifficulty))
            {
                return true;
            }
            return false;
        }
    }
}
