namespace Infrastructure.Service.Exception
{
    public class SyntaxException : System.Exception
    {
        public SyntaxException(string message) : base(message) { }
        public SyntaxException() : base("Incorrect syntax dynamic search.") { }
    }
}