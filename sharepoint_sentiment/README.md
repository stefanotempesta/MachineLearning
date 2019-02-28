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
  - Web (SharePoint Site)
  ```
  public class Web
  {
    public IList<List> Lists { get { return _lists; } }
    IList<List> _lists = new List<List>();
  }
  ```
  - List (SharePoint List)
  ```
  public class List
  {
     public IList<ListItem> GetItems(CamlQuery query)
     {
        return new List<ListItem> { new ListItem() };
     }
  }
  ```
  - ListItems
  ```
  public class ListItem
  {
     public object this[string index] =>_fields[index];
     IDictionary<string, object> _fields;
  }
  ```
  
