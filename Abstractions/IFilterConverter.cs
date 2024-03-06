using Infrastructure.Service.Model;

namespace Infrastructure.Services.Abstractions
{
    public interface IFilterConverter
    {
        void Deserialize<T>(string request) where T : class;
        CriteriaValue Compile();
    }
}