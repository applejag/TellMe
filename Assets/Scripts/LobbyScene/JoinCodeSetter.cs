using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinCodeSetter : MonoBehaviour
{
    public JoinCodeScript joinCodeScript;

    private void Start() {
        var joinCode = NetworkSessionData.joinCode?.ToUpper();
        Debug.Log($"Setting Join Code to '{joinCode}'");
        joinCodeScript.SetCode(joinCode);
    }
}
