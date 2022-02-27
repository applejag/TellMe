using NUnit.Framework;

public class FixedStringUtilTests
{
    [Test]
    public void TruncatesASCII()
    {
        var str = FixedStringUtil.CreateTruncated32("1234567890123456789012345678901234567890");
        // Truncates down to 29 bytes, as that's capacity of FixedString32Bytes
        Assert.AreEqual("12345678901234567890123456789", str.ToString());
    }

    [Test]
    public void TruncatesNonASCII()
    {
        // åäö are each 2 bytes long, so I'm replacing all of 123456 with åäö to account for this
        var str = FixedStringUtil.CreateTruncated32("åäö7890åäö7890åäö7890åäö7890");
        Assert.AreEqual("åäö7890åäö7890åäö789", str.ToString());
    }
}
