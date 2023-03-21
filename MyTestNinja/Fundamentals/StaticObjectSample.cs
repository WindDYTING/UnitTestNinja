using System.Threading;

namespace MyTestNinja.Fundamentals
{
    internal static class Global
    {
        public static int X = 10;
    }

    public class StaticObjectSample
    {
        public int ComputeSomething()
        {
            if (Global.X == 10)
            {
                _ = Interlocked.Increment(ref Global.X);
                return Global.X;
            }

            return Global.X;
        }
    }
}
