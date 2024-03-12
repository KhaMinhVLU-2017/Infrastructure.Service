using Infrastructure.Service.Model;

namespace Infrastructure.Service.Abstraction
{
    public interface IFilterConverter
    {
        void Deserialize<T>(string request) where T : class;
        CriteriaValue Compile();
    }
}