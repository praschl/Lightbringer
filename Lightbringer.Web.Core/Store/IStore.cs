using System.Collections.Generic;

namespace Lightbringer.Web.Store.Store
{
    public interface IStore
    {
        DaemonHost Get(int id);

        DaemonHost Find(string url);

        void Upsert(DaemonHost daemonHost);

        IReadOnlyCollection<DaemonHost> FindAllHosts();

        void Delete(int id);
    }
}