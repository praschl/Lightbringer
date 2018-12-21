using System.Threading.Tasks;

namespace Lightbringer.WebApi.ChangeNotification
{
    public interface IDaemonChangeDistributor
    {
        Task Distribute(string daemonName, string newState);
    }
}