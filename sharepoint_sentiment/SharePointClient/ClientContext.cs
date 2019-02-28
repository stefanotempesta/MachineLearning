using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SharePointClient
{
    public class ClientContext
    {
        public ClientContext(string url)
        {

        }

        public NetworkCredential Credentials { get; set; }

        public Web Web { get { return _web; } }

        public void Load(IList<List> lists)
        {
            lists.Add(new SharePointClient.List());
        }

        public void Load(IList<ListItem> listItems)
        {
            listItems.Add(new ListItem());
        }

        public void ExecuteQuery()
        {

        }

        public void Dispose()
        {

        }

        Web _web = new Web();
    }
}
