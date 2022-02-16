using UnityEngine;
using UnityEngine.Events;
using System;

public class OnSubmitScript : MonoBehaviour
{
    public OnSubmitEvent onSubmit = new OnSubmitEvent();

    public void Submit(string val) {
        if (onSubmit != null && Input.GetKey(KeyCode.Return)) {
            onSubmit.Invoke(val);
        }
    }

    [Serializable]
    public class OnSubmitEvent : UnityEvent<string> { }
}
