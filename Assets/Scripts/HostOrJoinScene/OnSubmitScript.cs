using UnityEngine;
using UnityEngine.Events;
using System;

public class OnSubmitScript : MonoBehaviour
{
    [Serializable]
    public class OnSubmitEvent : UnityEvent<string> { }

    public OnSubmitEvent onSubmit = new OnSubmitEvent();

    public void Submit(string val) {
        if (Input.GetKey(KeyCode.Return)) {
            onSubmit?.Invoke(val);
        }
    }
}
