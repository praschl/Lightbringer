namespace Lightbringer.Web.Store
{
    public interface IStore
    {
        ServiceHost Get(int id);

        ServiceHost Find(string url);

        void Upsert(ServiceHost serviceHost);
    }
}