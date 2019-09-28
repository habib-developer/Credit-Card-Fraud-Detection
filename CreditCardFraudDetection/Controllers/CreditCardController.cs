using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CreditCardFraudDetectionML.Model;
using Microsoft.ML;

namespace CreditCardFraudDetection.Controllers
{
    public class CreditCardController : Controller
    {
        //Dataset to use for predictions 
        private const string DATA_FILEPATH = @"C:\Users\mhabi\Desktop\creditcard.csv";
        [HttpGet]
        public IActionResult FraudPrediction()
        {
            ModelInput sampleData = CreateSingleDataSample(DATA_FILEPATH);
            return View(sampleData);
        }
        [HttpPost]
        public IActionResult FraudPrediction(ModelInput input)
        {
            ModelOutput prediction = ConsumeModel.Predict(input);
            ViewBag.Prediction = prediction;
            return View();
        }
        private static ModelInput CreateSingleDataSample(string dataFilePath)
        {
            // Create MLContext
            MLContext mlContext = new MLContext();

            // Load dataset
            IDataView dataView = mlContext.Data.LoadFromTextFile<ModelInput>(
                                            path: dataFilePath,
                                            hasHeader: true,
                                            separatorChar: ',',
                                            allowQuoting: true,
                                            allowSparse: false);

            // Use first line of dataset as model input
            // You can replace this with new test data (hardcoded or from end-user application)
            ModelInput sampleForPrediction = mlContext.Data.CreateEnumerable<ModelInput>(dataView, false)
                                                                        .First();
            return sampleForPrediction;
        }
    }
}
