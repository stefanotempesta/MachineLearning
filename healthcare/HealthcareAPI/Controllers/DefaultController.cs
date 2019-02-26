using HealthcareAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Healthcare.Controllers
{
    public class DefaultController : ApiController
    {
        // POST: api/Default
        public IHttpActionResult Post(string medicalRecordJson)
        {
            MedicalRecord medicalRecordObj = JsonConvert.DeserializeObject<MedicalRecord>(medicalRecordJson);

            Classification classification = new Classification();

            // Model training can be skipped when working on pre-trained models
            FileStream modelStream = new FileStream("[Path]", FileMode.Create);
            classification.TrainModel(new MedicalRecordDataSource(), modelStream);

            double accuracy = classification.EvaluateModel(new MedicalRecordDataSource(), modelStream);
            double score = classification.Score(medicalRecordObj, modelStream);
            RiskPrediction prediction = new RiskPrediction
            {
                Score = score,
                Accuracy = accuracy
            };

            return Json<RiskPrediction>(prediction);
        }
    }
}
