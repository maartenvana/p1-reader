using System;

namespace P1ReaderApp.Exceptions
{
    public class MessageParseException : Exception
    {
        public MessageParseException(string fieldName, string value, string typeName, Exception innerException)
            : base($"Unable to parse field {fieldName} with value {value} to {typeName}", innerException)
        {
        }
    }
}