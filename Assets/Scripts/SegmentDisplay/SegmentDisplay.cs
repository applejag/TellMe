using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SegmentDisplay : MonoBehaviour
{
    [Tooltip("Digits, left to right")]
    public SegmentDigit[] segmentDigits;
    public int maxValue = 1000;

    private void Reset()
    {
        maxValue = Mathf.FloorToInt(Mathf.Pow(10, maxValue));
    }

    public void SetValue(int val) {
        var digits = GetDigits(val % maxValue);

        for (int i = 0; i < digits.Length; i++) {
            segmentDigits[i].SetDigit(digits[i]);
        }
    }

    private int[] GetDigits(int num) {
        List<int> digits = new List<int>();
        while (num > 0) {
            digits.Add(num % 10);
            num = num / 10;
        }
        while (digits.Count < segmentDigits.Length) {
            digits.Add(0);
        }
        digits.Reverse();
        return digits.ToArray();
    }
}
