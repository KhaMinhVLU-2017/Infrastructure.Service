using Infrastructure.Service.Model;

namespace Infrastructure.Service.Abstraction
{
    public interface IValidation
    {
        void CanApply(BaseCriteria criteria);
    }
}