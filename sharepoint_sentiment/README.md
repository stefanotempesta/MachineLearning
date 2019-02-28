## SharePoint News Comments Sentiment Analysis
Technologies presented:
- SharePoint CSOM (Client Side Object Model)
- Azure Machine Learning

###  Visual Studio Solution
The Visual Studio 2017 solution in this folder contains the following projects:
- SharePointClient: Class library that implements models and functions for retrieving comments from news posted in a SharePoint site.
- SentimentAnalysis: Console app performs sentiment analysis of comments retrieved by using the SharePoint client library. Sentiment Analysis per performed in Azure Machine Learning and described on the [Azure AI Gallery](https://gallery.azure.ai/Experiment/SharePoint-Sentiment-Analysis).

### SharePoint Client
- Models
  - SocialComment
  ```
  public class SocialComment
  {
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
  public IEnumerable<string> Comments(ListItem item)
  ```

### Sentiment Analysis
- Program
  - RetrieveSharePointComments() returns a collection of comments retrieved from SharePoint, using the SharePoint client library.
  ```
  public IList<string> RetrieveSharePointComments()
  ```
  - InvokeMachineLearningService(comments) invokes the ML service hosted on Azure Machine Learning
  ```
  public async Task InvokeMachineLearningService(IEnumerable<string> comments)
  ```
  
