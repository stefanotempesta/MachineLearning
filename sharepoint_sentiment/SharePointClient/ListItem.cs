using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointClient
{
    public class ListItem
    {
        public object this[string index]
        {
            get
            {
                return _fields[index];
            }
        }

        IDictionary<string, object> _fields;

        public ListItem()
        {
            _fields = new Dictionary<string, object>();
            _fields.Add("Comments", new SocialComment[] {
                new SocialComment { Text = "Disappointed about Italy not making into the top 20... come on guys, that's shameful!" },
                new SocialComment { Text = "Glad to see the Netherlands topping this list, my grandma is from Amsterdam and she speaks excellent English." },
                new SocialComment { Text = "I was expecting the Nordic countries to be more proficient actually... but after all I see they are all there, top of the char anyway..." },
                new SocialComment { Text = "That's funny!" },
                new SocialComment { Text = ":-)" }
            });
        }
    }
}
