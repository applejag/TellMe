using System.Linq;
using Unity.Collections;

public static class FixedStringUtil
{
    // chars are 2 bytes long as .NET uses UTF-16
    private static readonly int maxCharsForString32Bytes = FixedString32Bytes.UTF8MaxLengthInBytes / 2;
    private static readonly int maxCharsForString64Bytes = FixedString64Bytes.UTF8MaxLengthInBytes / 2;
    private static readonly int maxCharsForString128Bytes = FixedString128Bytes.UTF8MaxLengthInBytes / 2;
    private static readonly int maxCharsForString512Bytes = FixedString512Bytes.UTF8MaxLengthInBytes / 2;
    private static readonly int maxCharsForString4096Bytes = FixedString4096Bytes.UTF8MaxLengthInBytes / 2;

    public static FixedString32Bytes CreateTruncated32(string value)
    {
        if (value == null)
        {
            return default;
        }
        if (value.Length < maxCharsForString32Bytes)
        {
            return value;
        }
        var str = new FixedString32Bytes();
        foreach (var c in value)
        {
            if (str.Append(c) == FormatError.Overflow)
            {
                break;
            }
        }
        return str;
    }

    public static FixedString64Bytes CreateTruncated64(string value)
    {
        if (value == null)
        {
            return default;
        }
        if (value.Length < maxCharsForString64Bytes)
        {
            return value;
        }
        var str = new FixedString64Bytes();
        foreach (var c in value)
        {
            if (str.Append(c) == FormatError.Overflow)
            {
                break;
            }
        }
        return str;
    }

    public static FixedString128Bytes CreateTruncated128(string value)
    {
        if (value == null)
        {
            return default;
        }
        if (value.Length < maxCharsForString128Bytes)
        {
            return value;
        }
        var str = new FixedString128Bytes();
        foreach (var c in value)
        {
            if (str.Append(c) == FormatError.Overflow)
            {
                break;
            }
        }
        return str;
    }

    public static FixedString512Bytes CreateTruncated512(string value)
    {
        if (value == null)
        {
            return default;
        }
        if (value.Length < maxCharsForString512Bytes)
        {
            return value;
        }
        var str = new FixedString512Bytes();
        foreach (var c in value)
        {
            if (str.Append(c) == FormatError.Overflow)
            {
                break;
            }
        }
        return str;
    }

    public static FixedString4096Bytes CreateTruncated4096(string value)
    {
        if (value == null)
        {
            return default;
        }
        if (value.Length < maxCharsForString4096Bytes)
        {
            return value;
        }
        var str = new FixedString4096Bytes();
        foreach (var c in value)
        {
            if (str.Append(c) == FormatError.Overflow)
            {
                break;
            }
        }
        return str;
    }
}
