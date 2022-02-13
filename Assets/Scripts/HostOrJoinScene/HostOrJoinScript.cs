using UnityEngine;

public class HostOrJoinScript : MonoBehaviour
{
    public RectTransform viewHome;
    public RectTransform viewHost;

    // Called by button onclick event
    public void ShowView(RectTransform view)
    {
        viewHome.anchorMin = new Vector2(-1, 0);
        viewHome.anchorMax = new Vector2(0, 1);
        viewHost.anchorMin = new Vector2(1, 0);
        viewHost.anchorMax = new Vector2(2, 1);

        view.anchorMin = new Vector2(0, 0);
        view.anchorMax = new Vector2(1, 1);
    }
}

