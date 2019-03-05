using CognitiveServicesClient;
using SharePointClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    private readonly string mlServiceEndpoint = ConfigurationManager.AppSettings["MLServiceEndpoint"];
    private readonly string mlApiKey = ConfigurationManager.AppSettings["MLApiKey"];
    private readonly string spSiteUrl = ConfigurationManager.AppSettings["SharePointSiteUrl"];
    private readonly string spUsername = ConfigurationManager.AppSettings["SharePointUsername"];
    private readonly string spPassword = ConfigurationManager.AppSettings["SharePointPassword"];

    static void Main(string[] args)
    {
        var p = new Program();

        // Retrieve a list of comments from a SharePoint site
        IEnumerable<SocialComment> spComments = p.RetrieveSharePointComments();

        // Transform the SharePoint comments into a dictionary [ID] = Text
        Dictionary<string, string> commentSet = new Dictionary<string, string>();
        foreach (var c in spComments)
        {
            commentSet.Add(c.Id, c.Text);
        }

        // Analyze the sentiment of each comment in the set
        IEnumerable<AnalyzedText> result = p.AnalyzeSentiment(commentSet).Result;

        // Display results for the analyzed sentiments
        p.DisplayResult(result);

        Console.ReadLine();
    }

    public IEnumerable<SocialComment> RetrieveSharePointComments()
    {
        List<SocialComment> comments = new List<SocialComment>();

        using (SharePointContext sp = new SharePointContext(spSiteUrl, spUsername, spPassword))
        {
            // Retrieve comments from each item in the "News" SharePoint list
            var news = sp.List("News");
            var items = sp.ListItems(news);

            foreach (var item in items)
            {
                var itemComments = sp.Comments(item);
                comments.AddRange(itemComments);
            }
        }

        return comments;
    }

    public async Task<IEnumerable<AnalyzedText>> AnalyzeSentiment(IDictionary<string,string> comments)
    {
        TextAnalyzer textAnalyzer = new TextAnalyzer();

        // First, identify language for each comment
        var languageComments = await textAnalyzer.DetectLanguageAsync(comments);

        // Return the sentiment for each comment
        return await textAnalyzer.SentimentAsync(languageComments);
    }

    private void DisplayResult(IEnumerable<AnalyzedText> result)
    {
        double avgSentiment = result.Average(c => c.Sentiment);
        double minSentiment = result.Min(c => c.Sentiment);
        double maxSentiment = result.Max(c => c.Sentiment);

        Console.WriteLine($"Average sentiment: {avgSentiment}");

        Console.WriteLine($"Comments with the lowest sentiment '{minSentiment}':");
        foreach (var comment in result.Where(c => c.Sentiment == minSentiment))
        {
            Console.WriteLine($"> {comment}");
        }

        Console.WriteLine($"Comments with the highest sentiment '{maxSentiment}':");
        foreach (var comment in result.Where(c => c.Sentiment == maxSentiment))
        {
            Console.WriteLine($"> {comment}");
        }
    }
}
