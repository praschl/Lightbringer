using System.Threading.Tasks;

namespace Lightbringer.WebApi.ChangeNotification
{
    public interface IDaemonChangeDistributor
    {
        Task Distribute(string type, string daemonName, string newState);
    }
}