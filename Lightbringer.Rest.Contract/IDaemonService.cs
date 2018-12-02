using System.Threading.Tasks;

namespace Lightbringer.Rest.Contract
{
    public interface IDaemonApi
    {
        Task<DaemonDto[]> GetAllDaemonsAsync();
    }
}
