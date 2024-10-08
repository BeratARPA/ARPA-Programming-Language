namespace ARPA_Programming_Language.Exceptions
{
    public class ReturnException : Exception
    {
        public object ReturnValue { get; }

        public ReturnException(object returnValue)
        {
            ReturnValue = returnValue;
        }
    }
}
