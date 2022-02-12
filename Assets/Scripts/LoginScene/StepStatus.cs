using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class StepStatus : MonoBehaviour
{
    public TMP_Text textStatus;
    public TMP_Text textMessage;

    [FormerlySerializedAs("colorOK")]
    public Color colorStatusOK = Color.green;
    public Color colorMessageOK = Color.white;
    [FormerlySerializedAs("colorError")]
    public Color colorStatusError = Color.red;
    public Color colorMessageError = Color.red;

    public void SetOK(string message)
    {
        SetOK();
        textMessage.text = message;
    }

    public void SetOK()
    {
        textStatus.text = "OK";
        textStatus.color = colorStatusOK;
        textMessage.color = colorMessageOK;
    }

    public void SetError()
    {
        textStatus.text = "ERR";
        textStatus.color = colorStatusError;
        textMessage.color = colorMessageError;
    }

    public void SetError(string message)
    {
        SetError();
        textMessage.text = message;
    }
}
