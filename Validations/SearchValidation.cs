using Infrastructure.Service.Model;
using Infrastructure.Service.Abstraction;

namespace Infrastructure.Service.Validation
{
    public class SearchValidation : IValidation
    {
        private ICompiler _compiler;
        private DynamicConfig _dynamicConfig;

        public SearchValidation(ICompiler compiler, DynamicConfig dynamicConfig)
        {
            _compiler = compiler;
            _dynamicConfig = dynamicConfig;
        }

        public void Validate(BaseCriteria criteria)
        {
            if (string.IsNullOrEmpty(criteria.Filters))
                return;

            // TODO Validate key if it hidden
        }
    }
}