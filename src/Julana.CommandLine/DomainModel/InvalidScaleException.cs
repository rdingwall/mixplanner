using System;

namespace Julana.CommandLine.DomainModel
{
    public class InvalidScaleException : Exception
    {
        public InvalidScaleException(string message) : base(message)
        {
        }
    }
}