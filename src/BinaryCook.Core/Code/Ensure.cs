using System;

namespace BinaryCook.Core.Code
{
    public class Ensure
    {
        public static void That(bool condition, string message)
        {
            Requires.That<ApplicationException>(condition, message);
        }
    }
}