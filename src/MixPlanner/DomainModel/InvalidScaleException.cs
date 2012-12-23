using System;

namespace MixPlanner.DomainModel
{
    [Serializable]
    public class InvalidScaleException : Exception
    {
        public InvalidScaleException(string message) : base(message)
        {
        }
    }
}