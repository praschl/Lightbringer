using System.Threading.Tasks;
using RestEase;

namespace Lightbringer.Rest.Contract
{
    public interface IIsAliveApi
    {
        [Get("isalive")]
        Task<bool> Get();
    }
}