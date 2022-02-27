using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class FixedStringTests
{
    [Test]
    public void ErrosWhenTooLongString()
    {
        Assert.Throws<ArgumentException>(
            () => new FixedString32Bytes("0123456789012345678901234567890123456789"));
    }

    [Test]
    public void OkWhenSubstringedWithASCII()
    {
        Assert.DoesNotThrow(
            () => new FixedString32Bytes("0123456789012345678901234567890123456789"[..FixedString32Bytes.UTF8MaxLengthInBytes]));
    }

    [Test]
    public void ErrorWhenSubstringedWithNonASCII()
    {
        Assert.Throws<ArgumentException>(
            () => new FixedString32Bytes("едц3456789едц3456789едц3456789едц3456789"[..FixedString32Bytes.UTF8MaxLengthInBytes]));
    }

    [Test]
    public void TestMaxLengthIs29()
    {
        // Mostly for documentation
        Assert.AreEqual(29, FixedString32Bytes.UTF8MaxLengthInBytes);
    }
}
