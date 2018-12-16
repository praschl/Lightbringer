using System.Threading.Tasks;
using RestEase;

namespace Lightbringer.Rest.Contract
{
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
