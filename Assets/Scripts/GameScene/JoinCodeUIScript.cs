using System.Collections;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class JoinCodeUIScript : MonoBehaviour
{
    public TMP_Text text;
    public CanvasGroup copyFeedback;
    public float copyFeedbackFadeTime = 0.25f;

    private void Start()
    {
        copyFeedback.alpha = 0;

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

        text.text = hostPlayer.joinCode.Value.ToString();
    }

    public void OnClickToCopyClicked()
    {
        GUIUtility.systemCopyBuffer = text.text;
        StopAllCoroutines();
        StartCoroutine(CrossFadeCopyFeedback());
    }

    private IEnumerator CrossFadeCopyFeedback()
    {
        var timeLeft = copyFeedbackFadeTime;
        while (timeLeft > 0)
        {
            copyFeedback.alpha = timeLeft / copyFeedbackFadeTime;
            timeLeft -= Time.deltaTime;
            yield return null;
        }
        copyFeedback.alpha = 0;
    }
}
