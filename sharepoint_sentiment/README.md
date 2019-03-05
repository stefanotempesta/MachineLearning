## SharePoint News Comments Sentiment Analysis
Technologies presented:
- SharePoint CSOM (Client Side Object Model)
- Azure Cognitive Services Text Analysis

###  Visual Studio Solution
The Visual Studio 2017 solution in this folder contains the following projects:
- *CognitiveServicesClient*: Class library that connects to Azure Cognitive Services for performing text analysy on a set of comments given in input. Operations supported are language detection and sentiment analysis.
- *SharePointClient*: Class library that implements models and functions for retrieving comments from news posted in a SharePoint site.
- *SentimentAnalysis*: Console app that performs sentiment analysis of comments. Comments are retrieved by using the SharePoint client library and sentiment is analyzed with the Cognitive Services client library.

### Cognitive Services Client
- Models
  - AnalyzedText
  ```
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
  ```
- Text Analyzer
  - DetectLanguageAsync(sentences) detects the language of a set of sentences.
  ```
  public async Task<IEnumerable<AnalyzedText>> DetectLanguageAsync(IDictionary<string, string> sentences)
  ```
  - SentimentAsync(languageSentences) detects the sentiment of a set of sentences according to a specific language.
  ```
  public async Task<IEnumerable<AnalyzedText>> SentimentAsync(IEnumerable<AnalyzedText> languageSentences)
  ```

### SharePoint Client
- Models
  - SocialComment
  ```
  public class SocialComment
  {
     public string Id { get; set; }
     public string Text { get; set; }
  }
  ```
- SharePoint Context
  - Lists() returns a list an enumeration of SharePoint lists
  ```
  public IEnumerable<List> Lists()
  ```
  - List(title) returns a specific list by its title
  ```
  public List List(string title)
  ```
  - ListItems(list) returns all items in the identified list
  ```
  public IEnumerable<ListItem> ListItems(List list)
  ```
  - Comments(item) returns all comments made for the identified list item
  ```
  public IEnumerable<SocialComment> Comments(ListItem item)
  ```

### Sentiment Analysis
- Program
  - RetrieveSharePointComments() returns a collection of comments retrieved from SharePoint, using the SharePoint client library.
  ```
  public IEnumerable<SocialComment> RetrieveSharePointComments()
  ```
  - AnalyzeSentiment(comments) analyzes sentiments for the identified comments, using the Cognitive Services client library.
  ```
  public async Task<IEnumerable<AnalyzedText>> AnalyzeSentiment(IDictionary<string,string> comments)
  ```
  
