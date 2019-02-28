using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointClient
{
    public class Web
    {
        public IList<List> Lists { get { return _lists; } }

        IList<List> _lists = new List<List>();
    }
}
