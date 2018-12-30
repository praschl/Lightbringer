using System.Threading.Tasks;

namespace Lightbringer.Web.Hubs
{
    public interface IDaemonHub
    {
        Task StateChanged(int hostId, string daemonName, string state);
    }
}