using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace HealthcareAPI.Models
{
    public class MedicalRecordDataSource : IDataSource<MedicalRecord>
    {
        public IEnumerable<MedicalRecord> ReadData(int split = 100)
        {
            IEnumerable<MedicalRecord> medicalRecords = new List<MedicalRecord>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = SqlQuery(split);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                medicalRecords.Append(ReadMedicalRecord(reader));
                            }
                        }
                    }
                }
            }

            return medicalRecords;
        }

        private MedicalRecord ReadMedicalRecord(SqlDataReader reader)
        {
            return new MedicalRecord
            {
                Age = reader.GetInt32(0),
                Sex = reader.GetInt32(1),
                Smoker = reader.GetBoolean(2),
                ChestPain = (ChestPainType)reader.GetInt32(3),
                BloodPressure = reader.GetInt32(4),
                SerumCholestoral = reader.GetInt32(5),
                FastingBloodSugar = reader.GetBoolean(6),
                MaxHeartRate = reader.GetInt32(7)
            };
        }

        private static string ConnectionString =>
            ConfigurationManager.ConnectionStrings["DataConnection"].ConnectionString;

        private static string SqlQuery(int split) =>
            $"SELECT [Age], [Sex], [Smoker], [ChestPain], [BloodPressure], [SerumCholestoral], [FastingBloodSugar], [MaxHeartRate] FROM [dbo].[MedicalRecords] tablesample({split} percent)";
    }
}
 