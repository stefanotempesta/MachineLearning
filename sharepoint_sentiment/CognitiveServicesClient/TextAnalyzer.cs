using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CognitiveServicesClient
{
    public class TextAnalyzer
    {
        private const string ApiEndpoint = "https://westus.api.cognitive.microsoft.com";
        private const string SubscriptionKey = "";

        private ITextAnalyticsClient _client;

        public TextAnalyzer()
        {
            _client = new TextAnalyticsClient(new ApiKeyServiceClientCredentials(SubscriptionKey))
            {
                Endpoint = ApiEndpoint
            };
        }

        public async Task<IEnumerable<AnalyzedText>> SentimentAsync(IEnumerable<AnalyzedText> languageSentences)
        {
            IList<MultiLanguageInput> batchInput = languageSentences
                .Select(s => new MultiLanguageInput
                {
                    Id = s.Id,
                    Text = s.Text,
                    Language = s.LanguageCode
                })
                .ToList();

            SentimentBatchResult result = await _client.SentimentAsync(new MultiLanguageBatchInput(batchInput));

            return result.Documents.Select(r => new AnalyzedText
            {
                Id = r.Id,
                Text = languageSentences.Single(s => s.Id == r.Id).Text,
                LanguageCode = languageSentences.Single(s => s.Id == r.Id).LanguageCode,
                Sentiment = r.Score.Value
            });
        }

        public async Task<IEnumerable<AnalyzedText>> DetectLanguageAsync(IDictionary<string, string> sentences)
        {
            IEnumerable<AnalyzedText> analyzedText = AnalyzedText.FromDictionary(sentences);

            IList<Input> batchInput = analyzedText
                .Select(t => new Input { Id = t.Id, Text = t.Text })
                .ToList();

            var result = await _client.DetectLanguageAsync(new BatchInput(batchInput));

            return result.Documents.Select(r => new AnalyzedText
            {
                Id = r.Id,
                Text = analyzedText.Single(t => t.Id == r.Id).Text,
                LanguageCode = r.DetectedLanguages[0].Iso6391Name,
                LanguageConfidence = r.DetectedLanguages[0].Score.Value
            });
        }
    }
}
