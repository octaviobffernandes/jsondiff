using JsonDiff.Models;

namespace JsonDiff.Services
{
    public interface IDiffService
    {
        bool ValidateInput(string data);

        DiffResult DiffJson(string left, string right);
    }
}
