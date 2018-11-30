using Lightbringer.Wcf.Contracts.Daemons;

namespace Lightbringer.Wcf.Daemons
{
    public class DaemonService : IDaemonService
    {
        public AllDaemonsResponse GetAllDaemons(AllDaemonsRequest request)
        {
            return new AllDaemonsResponse{ Daemons = {new DaemonDto{Description = "hi"} }};
        }
    }
}
