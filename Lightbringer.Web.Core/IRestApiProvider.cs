namespace Lightbringer.Web.Store
{
    public interface IRestApiProvider
    {
        T Get<T>(string url);
    }
}