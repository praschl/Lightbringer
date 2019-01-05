using System.Net.Http;
using System.Threading.Tasks;
using RestEase;

namespace Lightbringer.Rest.Contract
{
    // TODO: 1 move interfaces to .Web
    // TODO: 2 remove dependency on RestEase from this project.
    
    // TODO: integrate IsAlive into this IDaemonApi + Controller

    public interface IDaemonApi
    {
        [Get("daemons/find")]
        Task<DaemonDto[]> FindDaemons(string contains);

        [Get("daemons/details")]
        Task<DaemonDto[]> GetDaemons(string[] names);

        [Get("daemons/notify")]
        Task Notify(string url);

        [Get("daemons/start")]
        Task Start(string type, string name);

        [Get("daemons/stop")]
        Task Stop(string type, string name);
    }
}
