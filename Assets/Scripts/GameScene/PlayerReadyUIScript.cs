using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerReadyUIScript : MonoBehaviour
{
    private PlayerScript playerScript;
    public CountdownUIScript countdownUIScript;
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

        if (NetworkManager.Singleton.IsHost)
        {
            gameObject.SetActive(false);
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
        toggleButtonText.text = isReady ? readyText : notReadyText;
    }
}
