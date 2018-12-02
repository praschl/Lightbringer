using System.Threading.Tasks;
using Refit;

namespace Lightbringer.Rest.Contract
{
    public interface IDaemonApi
    {
        [Get("/daemons")]
        Task<DaemonDto[]> GetAllDaemonsAsync();
    }
}
