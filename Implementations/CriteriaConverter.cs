using System.Linq;
using System.Collections.Generic;
using Infrastructure.Service.Model;
using System.Text.RegularExpressions;

namespace Infrastructure.Service.Implementation
{
    public class CriteriaConverter
    {
        private const string AND_OPERATOR = "and";
        private const string OR_OPERATOR = "or";
        private const string RegexPattern = "@@";
        private int _index = 0;
        private List<string> _query = new List<string>();
        private List<object> _args = new List<object>();

        private string TranslateOperator(string operate)
        {
            switch (operate.Trim().ToLower())
            {
                case AND_OPERATOR:
                    return " && ";
                case OR_OPERATOR:
                    return " || ";
                default:
                    throw new System.Exception("Can't translate operate");
            }
        }

        public CriteriaValue Build()
        {
            string query = _query.First();
            _query.Clear();
            return new CriteriaValue(query);
        }

        public void Clear()
        {
            _index = 0;
            _args.Clear();
            _query.Clear();
        }

        public object[] GetArguments() => _args.ToArray();

        public void Append(object request)
        {
            if (request is CriteriaValue)
            {
                var item = request as CriteriaValue;
                var matches = Regex.Matches(item.Query, RegexPattern);
                if (!matches.Any())
                {
                    _query.Add(item.Query);
                    return;
                }
            }

            if (request.Equals(AND_OPERATOR) || request.Equals(OR_OPERATOR))
            {
                string result = "( " + string.Join(TranslateOperator(request as string), _query) + " )";
                _query.Clear();
                _query.Add(result);
                return;
            }
        }

        public CriteriaValue TransformQuery(CriteriaValue request)
        {
            if (request is null) return null;
            var matches = Regex.Matches(request.Query, RegexPattern);
            string query = request.Query;
            var args = new List<object>();
            for (int i = 0; i < matches.Count(); i++)
            {
                int index = query.IndexOf(RegexPattern);
                query = query.Remove(index, RegexPattern.Length);
                query = query.Insert(index, $"@{_index}");
                args.Add(request.Arguments[i]);
                _args.Add(request.Arguments[i]);
                _index++;
            }
            return new CriteriaValue(query, args);
        }

    }
}
