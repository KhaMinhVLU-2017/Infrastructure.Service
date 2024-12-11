namespace Infrastructure.Service.Model
{
    public class PropertyType
    {
        public string Name { get; set; }
        public bool IsCollection { get; set; }

        public PropertyType(string name, bool isCollection) => (Name, IsCollection) = (name, isCollection);
    }
}