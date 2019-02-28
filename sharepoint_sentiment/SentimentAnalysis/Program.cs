using SharePointClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
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

        var comments = p.RetrieveSharePointComments();
        p.InvokeMachineLearningService(comments)
            .Wait();
        
        Console.ReadLine();
    }

    public async Task InvokeMachineLearningService(IEnumerable<string> comments)
    {
        // Prepare input data
        var values = new string[1, comments.Count()];
        int i = 0;
        foreach (var comment in comments)
        {
            values.SetValue(comment, 1, i++);
        }

        // Invoke the ML service
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(mlServiceEndpoint);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", mlApiKey);

            var scoreRequest = new
            {
                Inputs = new Dictionary<string, StringTable>
                {
                    ["inputData"] = new StringTable
                    {
                        ColumnNames = new string[] { "TEXT" },
                        Values = values
                    }
                }
            };

            HttpResponseMessage response = await client.PostAsJsonAsync(string.Empty, scoreRequest);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Result: {0}", result);
            }
            else
            {
                Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));
                Console.WriteLine(response.Headers.ToString());

                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);
            }
        }
    }

    public IList<string> RetrieveSharePointComments()
    {
        List<string> comments = new List<string>();

        using (SharePointContext sp = new SharePointContext(spSiteUrl, spUsername, spPassword))
        {
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

    private struct StringTable
    {
        public string[] ColumnNames { get; set; }
        public string[,] Values { get; set; }
    }
}
