using System.Threading.Tasks;
using RestEase;

namespace Lightbringer.Rest.Contract
{
    public interface IDaemonApi
    {
        [Get("daemons")]
        Task<DaemonDto[]> GetDaemonsAsync(string contains);
    }
}
