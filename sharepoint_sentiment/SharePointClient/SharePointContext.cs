using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SharePointClient
{
    public class SharePointContext : IDisposable
    {
        public SharePointContext(string siteUrl, string username, string password)
        {
            _context = new ClientContext(siteUrl)
            {
                Credentials = new NetworkCredential(username, password)
            };
        }

        public IEnumerable<List> Lists()
        {
            Web web = _context.Web;
            _context.Load(web.Lists);
            _context.ExecuteQuery();

            return web.Lists.AsEnumerable();
        }

        public List List(string title)
        {
            Web web = _context.Web;
            _context.Load(web.Lists);
            _context.ExecuteQuery();

            List list = web.Lists.GetByTitle(title);

            return list;
        }

        public IEnumerable<ListItem> ListItems(List list)
        {
            CamlQuery query = CamlQuery.CreateAllItemsQuery();
            var listItems = list.GetItems(query);

            _context.Load(listItems);
            _context.ExecuteQuery();

            return listItems.AsEnumerable();
        }

        public IEnumerable<string> Comments(ListItem item)
        {
            var comments = item["Comments"] as SocialComment[];

            foreach (var comment in comments)
            {
                yield return comment.Text;
            }
        }

        private ClientContext _context = null;

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
