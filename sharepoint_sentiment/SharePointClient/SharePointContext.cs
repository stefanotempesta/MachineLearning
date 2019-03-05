using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Social;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

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

        public IEnumerable<SocialComment> Comments(ListItem item)
        {
            SocialFeedManager feedManager = new SocialFeedManager(_context);
            SocialFeedOptions feedOptions = new SocialFeedOptions();
            ClientResult<SocialFeed> feed = feedManager.GetFeedFor(_context.Web.CurrentUser.LoginName, feedOptions);
            _context.ExecuteQuery();

            foreach (SocialThread thread in feed.Value.Threads)
            {
                yield return new SocialComment
                {
                    Id = thread.Id,
                    Text = thread.RootPost.Text
                };

                foreach (SocialPost post in thread.Replies)
                {
                    yield return new SocialComment
                    {
                        Id = post.Id,
                        Text = post.Text
                    };
                }
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
