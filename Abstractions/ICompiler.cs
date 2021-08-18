using Infrastructure.Service.Model;

namespace Infrastructure.Service.Abstraction
{
    public interface ICompiler
    {
        string BuildQueryString(BaseCriteria criteria);
    }
}