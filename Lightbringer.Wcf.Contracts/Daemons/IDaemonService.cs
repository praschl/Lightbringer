using System.ServiceModel;

namespace Lightbringer.Wcf.Contracts.Daemons
{
    [ServiceContract(Namespace = Namespaces.Daemons)]
    public interface IDaemonService
    {
        [OperationContract]
        AllDaemonsResponse GetAllDaemons(AllDaemonsRequest request);
    }
}
