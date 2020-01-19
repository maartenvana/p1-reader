using System;

namespace P1ReaderApp.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class OBISField : Attribute
    {
        public string Format { get; set; }
        public string Reference { get; }

        public string ValueRegex { get; set; } = @"[\(](.*?)[\)]";

        public OBISField(string reference)
        {
            Reference = reference;
        }
    }
}