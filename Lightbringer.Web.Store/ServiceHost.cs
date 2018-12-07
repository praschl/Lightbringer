using System.Collections.Generic;

namespace Lightbringer.Web.Store
{
    public class ServiceHost
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public List<string> SubscribedServices { get; set; }= new List<string>();
    }
}
