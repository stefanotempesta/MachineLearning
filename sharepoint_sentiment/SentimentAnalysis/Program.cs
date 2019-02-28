using SharePointClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CallRequestResponseService
{
    class Program
    {
        static readonly string sharepointSiteUrl = ConfigurationManager.AppSettings["SharePointSiteUrl"];
        static readonly string serviceEndpoint = ConfigurationManager.AppSettings["ServiceEndpoint"];
        static readonly string apiKey = ConfigurationManager.AppSettings["ApiKey"];

        static void Main(string[] args)
        {
            InvokeMachineLearningService().Wait();

            Console.ReadLine();
        }

        static async Task InvokeMachineLearningService()
        {
            using (var client = new HttpClient())
            {
                var scoreRequest = new
                {
                    Inputs = new Dictionary<string, StringTable>() {
                        {
                            "input1",
                            new StringTable()
                            {
                                ColumnNames = new string[] { "TEXT" },
                                Values = new string[,] { { "I love SharePoint" } }
                            }
                        },
                    },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };
                
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                client.BaseAddress = new Uri(serviceEndpoint);

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

        static List<string> RetrieveSharePointComments()
        {
            List<string> comments = new List<string>();

            using (SharePointContext sp = new SharePointContext(sharepointSiteUrl, sharepointUsername, sharepointPassword))
            {
                List news = sp.List("News");
                var items = sp.ListItems(news);

                foreach (var item in items)
                {
                    var itemComments = sp.Comments(item);
                    foreach (var itemComment in itemComments)
                    {
                        comments.Add(itemComment);
                    }
                }
            }

            return comments;
        }

        static readonly string sharepointUsername = "stefano";
        static readonly string sharepointPassword = "Vegas*17";
    }

    public class StringTable
    {
        public string[] ColumnNames { get; set; }
        public string[,] Values { get; set; }
    }
}
