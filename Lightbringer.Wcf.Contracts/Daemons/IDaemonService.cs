using System.ServiceModel;

namespace Lightbringer.Wcf.Contracts.Daemons
{
    [ServiceContract(Namespace = Namespaces.Daemons)]
    public interface IDaemonService
    {
        [OperationContract]
        [ServiceKnownType(typeof(DaemonDto[]))]
        AllDaemonsResponse GetAllDaemons(AllDaemonsRequest request);
    }
}
