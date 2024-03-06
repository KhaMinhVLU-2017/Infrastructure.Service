using Infrastructure.Service.Model;
using Infrastructure.Service.Abstraction;

namespace Infrastructure.Service.Validation
{
    public class SearchValidation : IValidation
    {
        private DynamicConfig _dynamicConfig;

        public SearchValidation( DynamicConfig dynamicConfig)
        {

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