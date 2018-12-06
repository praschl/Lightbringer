using System.Threading.Tasks;
using RestEase;

namespace Lightbringer.Rest.Contract
{
    public interface IDaemonApi
    {
        [Get("daemons")]
        Task<DaemonDto[]> GetDaemons(string contains);
    }
}
