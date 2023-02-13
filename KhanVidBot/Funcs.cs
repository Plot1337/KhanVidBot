namespace KhanVidBot;

internal class Funcs
{
    public static string GetTs() => new DateTimeOffset(
        DateTime.UtcNow
        ).ToUnixTimeMilliseconds().ToString();

    public static string UintToStr(uint value)
    {
        int i = 32;
        var buffer = new char[i];
        int tbLen = _base.Length;

        do
        {
            buffer[--i] = _base[value % tbLen];
            value /= (uint)tbLen;
        }
        while (value > 0);

        var result = new char[32 - i];
        Array.Copy(buffer, i, result, 0, 32 - i);

        return new(result);
    }

    private static readonly char[] _base = {
        '0', '1', '2', '3', '4', '5',
        '6', '7', '8', '9', 'a', 'b',
        'c', 'd', 'e', 'f', 'g', 'h',
        'i', 'j', 'k', 'l', 'm', 'n',
        'o', 'p', 'q', 'r', 's', 't',
        'u', 'v'
    };
}