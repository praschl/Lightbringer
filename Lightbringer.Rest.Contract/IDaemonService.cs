using System.Threading.Tasks;

namespace Lightbringer.Rest.Contract
{
    public interface IDaemonService
    {
        Task<DaemonDto[]> GetAllDaemonsAsync();
    }
}
