using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HostGameScript : MonoBehaviour
{
    public ValidatedField fieldGameName;
    public ValidatedField fieldHostName;

    public Selectable[] disableOnLoad;

    public void OnHostGameClick()
    {
        var gameNameValid = fieldGameName.Validate();
        var hostNameValid = fieldHostName.Validate();

        if (!gameNameValid || !hostNameValid)
        {
            return;
        }

        SetFormInteractable(false);
        Debug.Log("host the game!");
    }

    private void SetFormInteractable(bool interactable)
    {
        foreach (var obj in disableOnLoad)
        {
            obj.interactable = interactable;
        }
    }
}
