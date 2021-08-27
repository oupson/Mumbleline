using System;

namespace Mumbleline.MumbleLink.FFI.Linux
{
    public class LinuxFFIException : Exception
    {
        private string methodName;
        private int errorCode;

        public LinuxFFIException(string methodName, int errorCode)
        {
            this.methodName = methodName;
            this.errorCode = errorCode;
        }

        public override string Message { get => string.Format("{0} (error code {1}) at {2}", Bindings.GetErrorCodeDescription(errorCode), errorCode, methodName); }
    }
}