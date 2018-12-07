namespace Lightbringer.Web.Store
{
    public interface IDaemonApiProvider
    {
        T Get<T>(string url);
    }
}