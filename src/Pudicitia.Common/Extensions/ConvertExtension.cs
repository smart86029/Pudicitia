using System;

namespace Pudicitia.Common.Extensions
{
    public static class ConvertExtension
    {
        // From:  To: Bol Chr SBy Byt I16 U16 I32 U32 I64 U64 Sgl Dbl Dec Dat Str
        // ----------------------------------------------------------------------
        // Boolean     x       x   x   x   x   x   x   x   x   x   x   x       x
        // Char            x   x   x   x   x   x   x   x   x                   x
        // SByte       x   x   x   x   x   x   x   x   x   x   x   x   x       x
        // Byte        x   x   x   x   x   x   x   x   x   x   x   x   x       x
        // Int16       x   x   x   x   x   x   x   x   x   x   x   x   x       x
        // UInt16      x   x   x   x   x   x   x   x   x   x   x   x   x       x
        // Int32       x   x   x   x   x   x   x   x   x   x   x   x   x       x
        // UInt32      x   x   x   x   x   x   x   x   x   x   x   x   x       x
        // Int64       x   x   x   x   x   x   x   x   x   x   x   x   x       x
        // UInt64      x   x   x   x   x   x   x   x   x   x   x   x   x       x
        // Single      x       x   x   x   x   x   x   x   x   x   x   x       x
        // Double      x       x   x   x   x   x   x   x   x   x   x   x       x
        // Decimal     x       x   x   x   x   x   x   x   x   x   x   x       x
        // DateTime                                                        x   x
        // String      x   x   x   x   x   x   x   x   x   x   x   x   x   x   x
        // ----------------------------------------------------------------------

        public static int ToInt(this bool value, int defaultValue = default)
        {
            return Convert.ToInt32(value);
        }

        public static int ToInt(this char value, int defaultValue = default)
        {
            return Convert.ToInt32(value);
        }

        public static int ToInt(this sbyte value, int defaultValue = default)
        {
            return Convert.ToInt32(value);
        }

        public static int ToInt(this int value, int defaultValue = default)
        {
            return Convert.ToInt32(value);
        }

        public static int ToInt(this uint value, int defaultValue = default)
        {
            return value <= int.MaxValue ? Convert.ToInt32(value) : default;
        }

        public static int ToInt(this long value, int defaultValue = default)
        {
            return int.MinValue <= value || value <= int.MaxValue ? Convert.ToInt32(value) : default;
        }

        public static int ToInt(this ulong value, int defaultValue = default)
        {
            return value <= int.MaxValue ? Convert.ToInt32(value) : default;
        }

        public static int ToInt(this float value, int defaultValue = default)
        {
            return TryConvert(() => Convert.ToInt32(value), defaultValue);
        }

        public static int ToInt(this double value, int defaultValue = default)
        {
            return TryConvert(() => Convert.ToInt32(value), defaultValue);
        }

        public static int ToInt(this decimal value, int defaultValue = default)
        {
            return TryConvert(() => Convert.ToInt32(value), defaultValue);
        }

        public static int ToInt(this string value, int defaultValue = default)
        {
            return TryConvert(() => Convert.ToInt32(value), defaultValue);
        }

        public static int ToInt(this object value, int defaultValue = default)
        {
            return TryConvert(() => Convert.ToInt32(value), defaultValue);
        }

        public static int ToInt(this DateTime value, int defaultValue = default)
        {
            return defaultValue;
        }

        private static TResult TryConvert<TResult>(Func<TResult> func, TResult defaultValue)
        {
            try
            {
                return func();
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}