using System.Runtime.Serialization;

namespace System.Linq.Dynamic.ApiFilter
{
    [Serializable]
    public class DynamicFilterException : Exception
    {
        public DynamicFilterException() : base( ) { }

        public DynamicFilterException(string message) : base( message ) { }

        public DynamicFilterException(string message, Exception innerException)
            : base( message, innerException )
        { }

        protected DynamicFilterException(SerializationInfo info, StreamingContext context)
            : base( info, context )
        { }
    }
}

