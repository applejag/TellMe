using UnityEngine;
using UnityEngine.Assertions;

public class SegmentDigit : MonoBehaviour
{
    public GameObject[] segments;
    public int[][] digits = new int[][]{
        new int[]{0, 1, 2, 4, 5, 6},    // 0
        new int[]{2, 5},                // 1
        new int[]{0, 2, 3, 4, 6},       // 2
        new int[]{0, 2, 3, 5, 6},       // 3
        new int[]{1, 2, 3, 5},          // 4
        new int[]{0, 1, 3, 5, 6},       // 5
        new int[]{0, 1, 3, 4, 5, 6},    // 6
        new int[]{0, 2, 5},             // 7
        new int[]{0, 1, 2, 3, 4, 5, 6}, // 8
        new int[]{0, 1, 2, 3, 5, 6},    // 9
    };

    public void SetDigit(int digit) {
        Assert.IsFalse(digit < 0, "digit cannot be less than 0");
        Assert.IsFalse(digit > 9, "digit cannot be more than 9");

        foreach (var seg in segments) {
            seg.SetActive(false);
        }
        
        foreach (var idx in digits[digit])
        {
            segments[idx].SetActive(true);
        }
    }
}
