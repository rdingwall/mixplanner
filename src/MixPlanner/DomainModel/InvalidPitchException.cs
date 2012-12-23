using System;

namespace MixPlanner.DomainModel
{
    [Serializable]
    public class InvalidPitchException : Exception
    {
        public InvalidPitchException(string message) : base(message)
        {
        }
    }
}