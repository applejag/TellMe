using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerReadyUIScript : MonoBehaviour
{
    private PlayerScript playerScript;
    public TMP_Text toggleButtonText;
    public string readyText = "Mark as not ready";
    public string notReadyText = "Mark as ready";

    void Start()
    {
        if (!NetworkManager.Singleton)
        {
            Debug.LogWarning("Lacking network manager.", this);
            enabled = false;
            return;
        }

        playerScript = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerScript>();
    }

    public void OnReadyToggleChanged(bool isReady)
    {
        if (!enabled)
        {
            return;
        }

        playerScript.SetPlayerIsReadyServerRpc(isReady);
    }
}
