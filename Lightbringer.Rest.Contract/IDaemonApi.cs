using System.Threading.Tasks;
using RestEase;

namespace Lightbringer.Rest.Contract
{
    public interface IDaemonApi
    {
        [Get("daemons")]
        Task<DaemonDto[]> FindDaemons(string contains);

        [Post("daemons")]
        Task<DaemonDto[]> GetDaemons([Body] string[] daemonNames);
    }
}
