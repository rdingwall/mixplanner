using System;

namespace MixPlanner.CommandLine.DomainModel
{
    public class InvalidScaleException : Exception
    {
        public InvalidScaleException(string message) : base(message)
        {
        }
    }
}