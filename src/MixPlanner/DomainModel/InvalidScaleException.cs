using System;

namespace MixPlanner.DomainModel
{
    public class InvalidScaleException : Exception
    {
        public InvalidScaleException(string message) : base(message)
        {
        }
    }
}