using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthcareAPI.Models
{
    public class RiskPrediction
    {
        [ColumnName("Score")]
        public double Score;

        public double Accuracy;
    }
}