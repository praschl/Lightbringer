using System.Collections.Generic;
using System.Threading.Tasks;
using Lightbringer.Rest.Contract;

namespace Lightbringer.WebApi
{
    public interface IDaemonProvider
    {
        Task<IEnumerable<DaemonDto>> FindDaemonsAsync(string contains);
        Task<IEnumerable<DaemonDto>> GetDaemonsAsync(string[] daemonNames);
    }
}