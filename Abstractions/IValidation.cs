using Infrastructure.Service.Model;

namespace Infrastructure.Service.Abstraction
{
    public interface IValidation
    {
        void Validate<T>(BaseCriteria criteria);
    }
}