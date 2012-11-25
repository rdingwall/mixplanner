using System;

namespace MixPlanner.CommandLine.DomainModel
{
    public class InvalidPitchException : Exception
    {
        public InvalidPitchException(string message) : base(message)
        {
        }
    }
}