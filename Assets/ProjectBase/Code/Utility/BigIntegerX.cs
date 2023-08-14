using System.Numerics;
using Numerics;

namespace Utility
{
    public static class BigIntegerX
    {
        public static BigInteger Clamp(BigInteger value, BigInteger min, BigInteger max)
        {
            return value < min ? min : value > max ? max : value;
        }

        public static float RationalDivisionAsFloat(BigInteger a, BigInteger b)
        {
            return (float)new BigRational(a, b);
        }
    }
}