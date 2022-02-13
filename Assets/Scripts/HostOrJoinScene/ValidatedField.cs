using TMPro;
using UnityEngine;

public class ValidatedField : MonoBehaviour
{
    public TMP_InputField field;
    public GameObject required;
    public bool hasBeenValidated;

    public bool Validate()
    {
        var valid = !string.IsNullOrWhiteSpace(field.text);
        required.SetActive(!valid);
        hasBeenValidated = true;
        return valid;
    }

    public void ValidateOnInputChange()
    {
        if (!hasBeenValidated)
        {
            return;
        }
        Validate();
    }
}
