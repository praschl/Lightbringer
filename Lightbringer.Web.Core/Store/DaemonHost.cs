using System.Collections.Generic;

namespace Lightbringer.Web.Core.Store
{
    public class DaemonHost
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        // TODO: SubscribedDaemons should be a struct that contains DaemonType and Name
        public List<string> SubscribedDaemons { get; set; }= new List<string>();
    }
}
