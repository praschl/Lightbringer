using System.Threading.Tasks;
using RestEase;

namespace Lightbringer.Rest.Contract
{
    public interface IDaemonApi
    {
        [Get("daemons")]
        Task<DaemonDto[]> GetDaemons(string contains);

        [Post("daemons")]
        Task<DaemonDto[]> GetDaemons(string[] daemonNames);
    }
}
