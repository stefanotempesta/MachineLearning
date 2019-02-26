using Microsoft.Data.DataView;
using Microsoft.ML;
using Microsoft.ML.Core.Data;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HealthcareAPI.Models
{
    public class Classification
    {
        public Classification()
        {
            _context = new MLContext();
        }

        public void TrainModel(IDataSource<MedicalRecord> dataSource, Stream targetStream)
        {
            IEnumerable<MedicalRecord> sourceData = dataSource.ReadData(80);  // 80% of records for training
            IDataView trainData = _context.Data.ReadFromEnumerable<MedicalRecord>(sourceData);
            
            var processChain = _context.Transforms.Concatenate(DefaultColumnNames.Features,
                nameof(MedicalRecord.Age),
                nameof(MedicalRecord.Sex),
                nameof(MedicalRecord.Smoker),
                nameof(MedicalRecord.ChestPain),
                nameof(MedicalRecord.BloodPressure),
                nameof(MedicalRecord.SerumCholestoral),
                nameof(MedicalRecord.FastingBloodSugar),
                nameof(MedicalRecord.MaxHeartRate)
            ).AppendCacheCheckpoint(_context);

            var trainer = _context.MulticlassClassification.Trainers.StochasticDualCoordinateAscent(labelColumn: DefaultColumnNames.Label, featureColumn: DefaultColumnNames.Features);
            var pipeline = processChain.Append(trainer);
            var model = pipeline.Fit(trainData);

            _context.Model.Save(model, targetStream);
        }

        public double EvaluateModel(IDataSource<MedicalRecord> dataSource, Stream modelStream)
        {
            IEnumerable<MedicalRecord> sourceData = dataSource.ReadData(20);  // 20% of records for evaluation
            var testData = _context.Data.ReadFromEnumerable<MedicalRecord>(sourceData);

            var predictions = _context.Model.Load(modelStream).Transform(testData);
            var metrics = _context.MulticlassClassification.Evaluate(data: predictions, label: DefaultColumnNames.Label, score: DefaultColumnNames.Score, predictedLabel: DefaultColumnNames.PredictedLabel, topK: 0);

            return metrics.TopKAccuracy / metrics.TopK * 100.0 / sourceData.Count();
        }

        public double Score(MedicalRecord medicalRecord, Stream modelStream)
        {
            ITransformer trainedModel = _context.Model.Load(modelStream);
            var predictionEngine = trainedModel.CreatePredictionEngine<MedicalRecord, RiskPrediction>(_context);

            var prediction = predictionEngine.Predict(medicalRecord);
            return prediction.Score;
        }

        private MLContext _context;
    }
}