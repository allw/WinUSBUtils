using System;

namespace WinUSBUtils
{
    public class EmptySerialNumberException : Exception
    {
        public EmptySerialNumberException(string message) : base(message)
        {
            
        }
    }
}
