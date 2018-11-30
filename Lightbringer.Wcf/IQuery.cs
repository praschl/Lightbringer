namespace Lightbringer.Wcf
{
    public interface IQuery<in TRequest, out TResponse>
    {
        TResponse Run(TRequest request);
    }
}