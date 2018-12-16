using System.Threading.Tasks;
using RestEase;

namespace Lightbringer.Rest.Contract
{
    // TODO: remove the interfaces and dependency on RestEase from this project.
    
    // TODO: integrate IsAlive into this IDaemonApi + Controller

    public interface IDaemonApi
    {
        [Get("daemons/find")]
        Task<DaemonDto[]> FindDaemons(string contains);

        [Get("daemons/details")]
        Task<DaemonDto[]> GetDaemons(string[] names);

        [Get("daemons/notify")]
        Task Notify(string url);
    }
}
