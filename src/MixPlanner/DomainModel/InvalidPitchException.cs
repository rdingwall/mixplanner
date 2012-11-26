using System;

namespace MixPlanner.DomainModel
{
    public class InvalidPitchException : Exception
    {
        public InvalidPitchException(string message) : base(message)
        {
        }
    }
}