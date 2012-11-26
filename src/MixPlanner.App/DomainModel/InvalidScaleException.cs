using System;

namespace MixPlanner.App.DomainModel
{
    public class InvalidScaleException : Exception
    {
        public InvalidScaleException(string message) : base(message)
        {
        }
    }
}