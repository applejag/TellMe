using TMPro;
using UnityEngine;

public class HostGameScript : MonoBehaviour
{
    public ValidatedField fieldGameName;
    public ValidatedField fieldHostName;

    public void OnHostGameClick()
    {
        var gameNameValid = fieldGameName.Validate();
        var hostNameValid = fieldHostName.Validate();

        if (!gameNameValid || !hostNameValid)
        {
            return;
        }

        Debug.Log("host the game!");
    }
}
