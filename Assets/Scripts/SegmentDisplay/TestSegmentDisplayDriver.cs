using UnityEngine;

public class TestSegmentDisplayDriver : MonoBehaviour
{
    public SegmentDisplay display;
    private void Update() {
        display.SetValue((int)(Time.time*50));
    }
}
