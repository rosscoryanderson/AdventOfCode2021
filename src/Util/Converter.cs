namespace src.Util;

public static class Converter
{
    public static long ToLong(this string binaryValue)
    {
        return Convert.ToInt64(binaryValue, 2);
    }
}