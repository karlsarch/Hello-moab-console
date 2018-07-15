using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Moab.Models
{
    public class Tour_Trail
    {
        public Trail Trail { get; set; }

        /// <summary>
        /// Indicates to the ride leader how difficult to make this trail ride.
        /// </summary>
        [Range(0,1000)]
        public int? Difficulty { get; set; }

        /// <summary>
        /// Provides an indication of which order the trails are meant to be ridden in; allows
        /// sorting.
        /// </summary>
        public int RideOrder { get; set; }

        /// <summary>
        /// Business logic to make sure we have a valid configuration; ensures MaxDifficulty >= MinDifficulty.
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            // TODO: Add implementation
            return false;
        }
    }
}
