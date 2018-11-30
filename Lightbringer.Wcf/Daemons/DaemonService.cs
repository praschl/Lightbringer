using System;
using Lightbringer.Wcf.Contracts.Daemons;

namespace Lightbringer.Wcf.Daemons
{
    public class DaemonService : IDaemonService
    {
        private readonly Func<IQuery<AllDaemonsRequest, AllDaemonsResponse>> _allDaemonsQuery;

        public DaemonService(Func<IQuery<AllDaemonsRequest, AllDaemonsResponse>> allDaemonsQuery)
        {
            _allDaemonsQuery = allDaemonsQuery;
        }

        public AllDaemonsResponse GetAllDaemons(AllDaemonsRequest request)
        {
            return _allDaemonsQuery().Run(request);
        }
    }
}