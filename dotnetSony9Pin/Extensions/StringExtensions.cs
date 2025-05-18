namespace dotNetSony9Pin.Extenions;

public static class StringExtensions
{
    public static string FixedLength(this string value, int totalWidth, char paddingChar = ' ')
    {
        if (value is null)
            return new string(paddingChar, totalWidth);

        if (value.Length > totalWidth)
            return value.Substring(0, totalWidth);
        else
            return value.PadRight(totalWidth, paddingChar);
    }

}
