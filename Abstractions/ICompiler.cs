using Infrastructure.Service.Model;

namespace Infrastructure.Service.Abstraction
{
    public interface ICompiler
    {
        T DeserializeModel<T>(string filters);
        bool IsHasOperateAndParser(Criteria criteria);
        string BuildQueryString(BaseCriteria criteria);
    }
}