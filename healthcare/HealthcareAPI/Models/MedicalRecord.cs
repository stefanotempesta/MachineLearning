using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthcareAPI.Models
{
    public class MedicalRecord
    {
        public int Age { get; set; }

        public int Sex { get; set; }                        // 0 = Female, 1 = Male

        public bool Smoker { get; set; }

        public ChestPainType ChestPain { get; set; }

        public int BloodPressure { get; set; }              // in mm Hg on admission to the hospital

        public int SerumCholestoral { get; set; }           // in mg/dl

        public bool FastingBloodSugar { get; set; }         // true if > 120 mg/dl

        public int MaxHeartRate { get; set; }               // maximum heart rate achieved
    }

    public enum ChestPainType
    {
        TypicalAngina = 1,
        AtypicalAngina,
        NonAnginal,
        Asymptomatic
    }
}