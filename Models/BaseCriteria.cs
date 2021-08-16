namespace Infrastructure.Service.Model
{
    public class BaseCriteria
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string Filters { get; set; }
        public string Sorts { get; set; }
    }
}