namespace Infrastructure.Service.Abstraction
{
    public interface ISortConverter
    {
        public string Compile();
        public void Deserialize(string request);
    }
}