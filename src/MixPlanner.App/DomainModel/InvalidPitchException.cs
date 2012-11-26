using System;

namespace MixPlanner.App.DomainModel
{
    public class InvalidPitchException : Exception
    {
        public InvalidPitchException(string message) : base(message)
        {
        }
    }
}