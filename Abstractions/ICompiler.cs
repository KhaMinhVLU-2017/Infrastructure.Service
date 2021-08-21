using Infrastructure.Service.Model;

namespace Infrastructure.Service.Abstraction
{
    public interface ICompiler
    {
        Criteria DeserializeFilter(string filters);
        bool IsHasOperateAndParser(Criteria criteria);
        string BuildQueryString(BaseCriteria criteria);
    }
}