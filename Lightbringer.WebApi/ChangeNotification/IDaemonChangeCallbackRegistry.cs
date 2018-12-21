namespace Lightbringer.WebApi.ChangeNotification
{
    public interface IDaemonChangeCallbackRegistry
    {
        void RegisterCallbackUrl(string url);
    }
}