using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moab.Models.Helpers
{
    public class RTTLHelper
    {
        public static int CalculateTargetStandupReps(int duration, int age, char gender)
        {
            gender = Char.ToLower(gender);
            int thirtySecondGoal;
            switch (gender)
            {
                case 'm':
                    thirtySecondGoal = CalculateThirtySecondTarget(age,gender);
                    break;
                case 'f':
                    thirtySecondGoal = CalculateThirtySecondTarget(age,gender);
                    break;
                default:
                    throw new ArgumentException("please use only 'm' for male and 'f' for female");
            }
            int finalGoal = ModifyTargetByTime(thirtySecondGoal, duration);
            return finalGoal;

        }

        private static int CalculateThirtySecondTarget(int age, char gender)
        {
            int target;
            if (age <= 64)
            {
                target = (gender == 'm') ? 14 : 12;
                return target;
            }
            if (age <= 69)
            {
                target = (gender == 'm') ? 12 : 11;
                return target;
            }
            if (age <= 74)
            {
                target = (gender == 'm') ? 12 : 10;
                return target;
            }
            if (age <= 79)
            {
                target = (gender == 'm') ? 11 : 10;
                return target;
            }
            if (age <= 84)
            {
                target = (gender == 'm') ? 10 : 9;
                return target;
            }
            if (age <= 89)
            {
                target = (gender == 'm') ? 8 : 8;
                return target;
            }
            if (age <= 90)
            {
                target = (gender == 'm') ? 7 : 4;
                return target;
            }
            else
            {
                throw new Exception();
            }
        }


        private static int ModifyTargetByTime(int thirtySecondGoal, int duration)
        {
            double finalGoal;
            if (duration <= 0)
            {
                throw new ArgumentException("Really? A negative time? Come on.");
            }
            else if (duration <= 30)
            {
                finalGoal = thirtySecondGoal * (duration / 30.0);
                return (int)finalGoal;
            }
            else
            {
                finalGoal = thirtySecondGoal + thirtySecondGoal * ((duration - 30.0) / 60.0);
                return (int)finalGoal;
            }
        }
    }
}
