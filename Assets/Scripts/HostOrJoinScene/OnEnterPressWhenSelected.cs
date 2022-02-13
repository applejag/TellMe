using UnityEngine;
using UnityEngine.Events;
using System;

public class OnEnterPressWhenSelected : MonoBehaviour
{
    [Serializable]
    public class OnEnterKeyPressEvent : UnityEvent<string> { }

    public OnEnterKeyPressEvent onEnterKeyPress;

    private TMPro.TMP_InputField inputField;

    private void Start() {
        inputField = GetComponent<TMPro.TMP_InputField>();
    }

    private void Update() {
        if (onEnterKeyPress != null && inputField.isFocused && Input.GetKeyDown(KeyCode.Return)) {
            onEnterKeyPress.Invoke(inputField.text);
        }
    }
}
