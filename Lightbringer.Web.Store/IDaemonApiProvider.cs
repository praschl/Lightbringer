using Lightbringer.Rest.Contract;

namespace Lightbringer.Web.Store
{
    public interface IDaemonApiProvider
    {
        IDaemonApi GetDaemonApi(string url);
        IIsAliveApi GetIsAliveApi(string url);
    }
}