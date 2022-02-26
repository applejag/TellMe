using Unity.Netcode;
using UnityEngine;

public class InitialNameChangeScript : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public Animator animator;
    public string hideTrigger = "Hide";
    public ValidatedField nameField;
    private PlayerScript playerScript;

    private void Reset()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (!NetworkManager.Singleton)
        {
            Debug.LogWarning("Lacking network manager.", this);
            enabled = false;
            return;
        }

        if (NetworkManager.Singleton.IsServer)
        {
            gameObject.SetActive(false);
            return;
        }

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;

        playerScript = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerScript>();
    }

    public void OnConfirmNameClicked()
    {
        if (!nameField.Validate())
        {
            return;
        }
        playerScript.SetPlayerNameServerRpc(nameField.field.text);
        animator.SetTrigger(hideTrigger);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
