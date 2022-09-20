using System;
using System.Collections.Generic;

namespace FitnessManagementSystem.Models
{
    public class Exercise : CommonFeatures
    {
        public string ExerciseName;

        public int ExerciseDuration;

        public int ExerciseDifficulty;

        public int ExerciseArea;

        public List<Movement> Movements;
    }
}

