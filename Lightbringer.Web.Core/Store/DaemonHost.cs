using System.Collections.Generic;

namespace Lightbringer.Web.Store.Store
{
    public class DaemonHost
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public List<string> SubscribedDaemons { get; set; }= new List<string>();
    }
}
