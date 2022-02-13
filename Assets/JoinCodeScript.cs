using UnityEngine;

public class JoinCodeScript : MonoBehaviour
{
    public TMPro.TMP_InputField codeText;
    public GameObject copyButton;

    public void SetCode(string code) {
        codeText.text = code;
        copyButton.SetActive(true);
    }

    public void CopyCode() {
        GUIUtility.systemCopyBuffer = codeText.text;
    }
}
