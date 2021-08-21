namespace Infrastructure.Service.Exception
{
    public class SyntaxException : System.Exception
    {
        public SyntaxException() : base("Incorrect syntax dynamic search.") { }
    }
}