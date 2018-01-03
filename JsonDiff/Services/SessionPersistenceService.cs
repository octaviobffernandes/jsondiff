using Microsoft.AspNetCore.Http;

namespace JsonDiff.Services
{
    /// <summary>
    /// Session based persistence service
    /// </summary>
    public class SessionPersistenceService : IPersistenceService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private ISession _session => _contextAccessor.HttpContext.Session;

        public SessionPersistenceService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string Get(string key)
        {
            return _session.GetString(key);
        }

        public void Save(string key, string value)
        {
            _session.SetString(key, value);
        }
    }
}
