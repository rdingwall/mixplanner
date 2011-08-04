using System;

namespace Julana.CommandLine.DomainModel
{
    public class InvalidPitchException : Exception
    {
        public InvalidPitchException(string message) : base(message)
        {
        }
    }
}