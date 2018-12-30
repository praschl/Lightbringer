using Microsoft.AspNetCore.SignalR;

namespace Lightbringer.Web.Hubs
{
    public class DaemonHub : Hub<IDaemonHub>
    {
        // we need a hub for configuration and all, but we dont need any methods here,
        // because we wont do any client-to-server calls (for now).
    }
}
