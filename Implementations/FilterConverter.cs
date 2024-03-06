using System.Linq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Infrastructure.Service.Model;
using Infrastructure.Services.Abstractions;

namespace Infrastructure.Service.Implementation
{
    public class FilterConverter: IFilterConverter
    {
        private const string AND_OPERATOR = "and";
        private const string OR_OPERATOR = "or";
        private Stack<object> _stackCriteria = new Stack<object>();
        private CriteriaConverter _criteriaConverter = new CriteriaConverter();
        private CriteriaValueBuilder _criteriaValueBuilder = new CriteriaValueBuilder();

        public CriteriaValue Compile()
        {
            if (_stackCriteria.Count == 0) return null;

            var item = _stackCriteria.Pop() as CriteriaValue;
            string result = item.Query;
            var args = _criteriaConverter.GetArguments();
            _criteriaConverter.Clear();
            return new CriteriaValue(result, args);
        }

        public void Deserialize<T>(string request) where T : class
        {
            _criteriaValueBuilder.AppendEntityType(typeof(T));
            Deserialize(request);
        }

        private void Deserialize(string request)
        {
            if (string.IsNullOrEmpty(request)) return;

            bool isArray = TryParseJArray(request, out JArray criteriaArray);
            if (isArray)
            {
                foreach (var item in criteriaArray)
                    Deserialize(item.ToString());

                int count = criteriaArray.Count();
                var criteria = Serializate(count);
                _stackCriteria.Push(criteria);
                return;
            }

            JToken criterias = JToken.Parse(request);
            var andOperator = criterias[AND_OPERATOR];
            var orOperator = criterias[OR_OPERATOR];

            if (andOperator is null && orOperator is null)
            {
                _criteriaValueBuilder.AppendCriteria(request);
                var criteria = _criteriaValueBuilder.Build();
                var result = _criteriaConverter.TransformQuery(criteria);
                _stackCriteria.Push(result);
                return;
            }

            if (andOperator != null)
            {
                _stackCriteria.Push(AND_OPERATOR);
                Deserialize(andOperator.ToString());
            }

            if (orOperator != null)
            {
                _stackCriteria.Push(OR_OPERATOR);
                Deserialize(orOperator.ToString());
            }
        }

        private CriteriaValue Serializate(int count)
        {
            for (int i = 0; i <= count; i++)
                _criteriaConverter.Append(_stackCriteria.Pop());

            var criteria = _criteriaConverter.Build();
            return criteria;
        }

        private bool TryParseJArray(string request, out JArray result)
        {
			result = default(JArray);
			try
            {
				bool hasArray = request.StartsWith("[") && request.EndsWith("]");
				if (!hasArray) return false;

				JArray criteriaArray = JArray.Parse(request);
                result = criteriaArray;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
