using System.Collections.Generic;
using Lightbringer.Rest.Contract;

namespace Lightbringer.Web.Store
{
    public interface IDaemonApiProvider
    {
        IDaemonApi Get(string url);

        IReadOnlyCollection<IDaemonApi> GetAll();
    }
}