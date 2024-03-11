using Newtonsoft.Json.Linq;

namespace Infrastructure.Service.Helper
{
    public static class JsonHelper
    {
        public static bool TryParseJArray(string request, out JArray result)
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