using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class JoinCodeUIScript : MonoBehaviour
{
    public TMP_Text text;

    private void Start()
    {
        if (!NetworkManager.Singleton)
        {
            Debug.LogWarning("Lacking network manager.", this);
            return;
        }

        var hostPlayer = FindObjectsOfType<PlayerScript>().Where(p => p.IsOwnedByServer).FirstOrDefault();
        if (!hostPlayer)
        {
            Debug.LogWarning("Unable to find host player.", this);
            return;
        }

        text.text = hostPlayer.joinCode.Value.Value;
    }

    public void OnClickToCopyClicked()
    {
        GUIUtility.systemCopyBuffer = text.text;
    }
}
