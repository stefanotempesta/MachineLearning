using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointClient
{
    public class List
    {
        public IList<ListItem> GetItems(CamlQuery query)
        {
            return new List<ListItem> { new ListItem() };
        }
    }

    public static class ListExtensions
    {
        public static List GetByTitle (this IList<List> list, string title)
        {
            return new List();
        }
    }
}
