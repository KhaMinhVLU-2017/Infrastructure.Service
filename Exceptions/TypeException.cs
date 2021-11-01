namespace Infrastructure.Service.Exception
{
    public class TypeException : System.Exception
    {
        public TypeException() : base("Incorrect type value.") { }
    }
}