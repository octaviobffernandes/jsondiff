using JsonDiff.Models;

namespace JsonDiff.Services
{
    public interface IPersistenceService
    {
        void Save(string key, string value);
        string Get(string key);
    }
}
