## Secure Multiparty Machine Learning with Azure Confidential Computing

Technologies presented:
- Azure Confidential Computing
- SQL Server Always Encrypted with Secure Enclaves
- ML.NET Multiclass Classification

### Azure Confidential Computing
[Azure Confidential Computing](https://azure.microsoft.com/en-us/solutions/confidential-compute/) adds new data security capabilities using trusted execution environments (TEEs) to protect your data while in use.

### SQL Server Always Encrypted with Secure Enclaves
[SQL Server 2019 Always Encrypted with Secure Enclaves](https://aka.ms/AlwaysEncryptedwithSecureEnclaves) introduces a secure enclave as a protected region of memory within the SQL Server process, and acts as a trusted execution environment for processing sensitive data inside the SQL Server engine.

### ML.NET Multiclass Classification
[ML.NET](https://dot.net/ml) is an open source and cross-platform machine learning framework for .NET that runs machine learning tasks like classification and regression.

## Visual Studio Solution
The Visual Studio 2017 solution in this folder contains the following objects:
- DefaultController: Web API controller that implements the POST HTTP method only.
  ```
  public IHttpActionResult Post(string medicalRecordJson)
  ```
  - Input: A medical record, in JSON format.
  ```
  {
  "age": 0,
  "sex": 0,
  "smoker": false,
  "chestPain": 0,
  "bloodPressure": 0,
  "serumCholestoral": 0,
  "fastingBloodSugar": false,
  "maxHeartRate": 0
  }
  ```
  - Output: A JSON object containing the predicted score and accuracy.
  ```
  {
  "score": 0.0,
  "accuracy": 0.0
  }
  ```
- Classification: C# class that implements the multiclass classification algorithm with ML.NET
  - TrainModel
  ```
  public void TrainModel(IDataSource<MedicalRecord> dataSource, Stream targetStream)
  ```
  - EvaluateModel
  ```
  public double EvaluateModel(IDataSource<MedicalRecord> dataSource, Stream modelStream)
  ```
  - Score
  ```
  public double Score(MedicalRecord medicalRecord, Stream modelStream)
  ```
- MedicalRecord: Input model
- RiskPrediction: Output model
- MedicalRecordDataSource: Connection to SQL Server using a SQL client driver
