using System;
using System.Collections.Concurrent;
using RestEase;

namespace Lightbringer.Web.Store
{
    public class RestApiProvider : IRestApiProvider
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<Type, object>> _apisByUrl = new ConcurrentDictionary<string, ConcurrentDictionary<Type, object>>(StringComparer.OrdinalIgnoreCase);

        public T Get<T>(string url)
        {
            var apiType = typeof(T);

            var apisByType = _apisByUrl.GetOrAdd(url, u => new ConcurrentDictionary<Type, object>());

            var instance = apisByType.GetOrAdd(apiType, t => RestClient.For<T>(url));

            return (T) instance;
        }
    }
}