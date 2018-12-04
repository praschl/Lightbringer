using System.Collections.Generic;

namespace Lightbringer.Web.Store
{
    public interface IStore
    {
        void AddServiceHost(string name, string url);

        IReadOnlyCollection<string> GetServiceHosts();
    }
}