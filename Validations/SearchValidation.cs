using Infrastructure.Service.Model;
using Infrastructure.Service.Exception;
using Infrastructure.Service.Abstraction;

namespace Infrastructure.Service.Validation
{
    public class SearchValidation : IValidation
    {
        private ICompiler _compiler;
        private DynamicSearch _dynamicConfig;

        public SearchValidation(ICompiler compiler, DynamicSearch dynamicConfig)
        {
            _compiler = compiler;
            _dynamicConfig = dynamicConfig;
        }

        public void Validate(BaseCriteria criteria)
        {
            var criteriaFilter = _compiler.DeserializeFilter(criteria.Filters);

            if (!_compiler.IsHasOperateAndParser(criteriaFilter))
                throw new SyntaxException();

            if (!IsTypeAllowOperates(criteriaFilter))
                throw new OperateException(criteriaFilter.OperateType, criteriaFilter.Operate);

            // TODO validate type apply | convert type correct type
        }

        private bool IsTypeAllowOperates(Criteria criteria)
        {
            bool isAllow = true;
            // Need check type not in dic
            bool isHaveOperate = _dynamicConfig.LimitOperates.TryGetValue(criteria.OperateType, out string bounchOperate);
            if (!isHaveOperate || string.IsNullOrEmpty(bounchOperate)) // Mean not config limit operate
                return isAllow;

            if (!bounchOperate.Contains(criteria.Operate))
                isAllow = false;

            return isAllow;
        }
    }
}