using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CognitiveServicesClient
{
    public class AnalyzedText
    {
        public string Id { get; internal set; }

        public string Text { get; internal set; }

        public string LanguageCode { get; internal set; }

        public double LanguageConfidence { get; internal set; }

        public double Sentiment { get; internal set; }

        public static IEnumerable<AnalyzedText> FromDictionary(IDictionary<string, string> sentences) =>
            sentences.Keys.Select(key => new AnalyzedText { Id = key, Text = sentences[key] });
    }
}
