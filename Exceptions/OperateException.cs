namespace Infrastructure.Service.Exception
{
    public class OperateException : System.Exception
    {
        public OperateException(string type, string operate) : base($"The {type} type not support {operate} operate.")
        {
        }
    }
}