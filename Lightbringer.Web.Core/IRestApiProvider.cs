namespace Lightbringer.Web.Core
{
    public interface IRestApiProvider
    {
        T Get<T>(string url);
    }
}